using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PostProcessingEditor
{
    public class GlobalVolumeEditorWindow : EditorWindow
    {
        private const string PrefKey = "GlobalVolumeEditor_ProfileGUID";

        private GameObject _volumeGO;
        private Volume _volume;

        private VolumeProfile _profile;

        private readonly List<Editor> _componentEditors = new List<Editor>();
        private readonly Dictionary<string, bool> _foldouts = new Dictionary<string, bool>();

        private Vector2 _scroll;

        private static readonly Type GameViewType =
            Type.GetType("UnityEditor.GameView,UnityEditor");

        [MenuItem("Tools/Lugu/Global Volume Editor")]
        public static void Open()
        {
            GetWindow<GlobalVolumeEditorWindow>("Global Volume Editor");
        }

        private void OnEnable()
        {
            RestoreProfile();
            CreateVolume();
        }

        private void OnDisable()
        {
            SaveProfile();
            ClearComponentEditors();
            DestroyVolume();
        }

        // -----------------------------------------------------------------------
        // Persistence

        private void RestoreProfile()
        {
            string guid = EditorPrefs.GetString(PrefKey, string.Empty);
            if (string.IsNullOrEmpty(guid)) return;

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return;

            _profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
        }

        private void SaveProfile()
        {
            if (_profile == null)
            {
                EditorPrefs.DeleteKey(PrefKey);
                return;
            }

            string path = AssetDatabase.GetAssetPath(_profile);
            string guid = AssetDatabase.AssetPathToGUID(path);
            EditorPrefs.SetString(PrefKey, guid);
        }

        // -----------------------------------------------------------------------
        // Volume lifecycle

        private void CreateVolume()
        {
            float highestPriority = 0f;
            foreach (var v in FindObjectsByType<Volume>(FindObjectsSortMode.None))
                if (v.priority > highestPriority)
                    highestPriority = v.priority;

            _volumeGO = new GameObject("[GlobalVolumeEditor] Temp Volume")
            {
                hideFlags = HideFlags.DontSave
            };

            _volume = _volumeGO.AddComponent<Volume>();
            _volume.isGlobal = true;
            _volume.priority = highestPriority + 1f;

            ApplyProfileToVolume();
        }

        private void DestroyVolume()
        {
            if (_volumeGO != null)
                DestroyImmediate(_volumeGO);

            _volume = null;
        }

        private void ApplyProfileToVolume()
        {
            if (_volume == null) return;

            _volume.profile = _profile;
            RebuildComponentEditors();
            RepaintViews();
        }

        // -----------------------------------------------------------------------
        // Component editors

        private void RebuildComponentEditors()
        {
            ClearComponentEditors();

            if (_profile == null) return;

            foreach (var component in _profile.components)
            {
                if (component == null) continue;
                _componentEditors.Add(Editor.CreateEditor(component));
            }
        }

        private void ClearComponentEditors()
        {
            foreach (var e in _componentEditors)
                if (e != null) DestroyImmediate(e);

            _componentEditors.Clear();
        }

        // -----------------------------------------------------------------------

        private void OnGUI()
        {
            DrawCameraWarning();
            EditorGUILayout.Space(4);
            DrawProfileField();
            EditorGUILayout.Space(4);

            if (_profile == null)
            {
                EditorGUILayout.HelpBox("Drag a Volume Profile above to begin editing.", MessageType.Info);
                return;
            }

            DrawVolumeControls();

            if (_volume == null)
                return;

            EditorGUILayout.Space(4);

            // Rebuild if the profile's component list changed externally
            if (_profile.components.Count != _componentEditors.Count)
                RebuildComponentEditors();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            EditorGUI.BeginChangeCheck();

            foreach (var editor in _componentEditors)
            {
                if (editor == null) continue;

                var component = (VolumeComponent)editor.target;
                string key = component.GetType().Name;

                if (!_foldouts.ContainsKey(key))
                    _foldouts[key] = true;

                // Custom header: toggle (active) + foldout label
                Rect headerRect = GUILayoutUtility.GetRect(0f, EditorGUIUtility.singleLineHeight + 4f);
                headerRect = EditorGUI.IndentedRect(headerRect);

                Rect toggleRect  = new Rect(headerRect.x + 2f,  headerRect.y + 2f, 16f, 16f);
                Rect foldoutRect = new Rect(headerRect.x + 22f, headerRect.y,      headerRect.width - 22f, headerRect.height);

                // Draw header background matching the foldout header style
                if (Event.current.type == EventType.Repaint)
                    EditorStyles.foldoutHeader.Draw(headerRect, false, false, _foldouts[key], false);

                bool newActive = EditorGUI.Toggle(toggleRect, component.active);
                if (newActive != component.active)
                {
                    Undo.RecordObject(component, "Toggle Volume Component");
                    component.active = newActive;
                    EditorUtility.SetDirty(component);
                }

                using (new EditorGUI.DisabledScope(!component.active))
                    _foldouts[key] = EditorGUI.Foldout(foldoutRect, _foldouts[key],
                        ObjectNames.NicifyVariableName(key), true, EditorStyles.foldoutHeader);

                if (_foldouts[key] && component.active)
                {
                    editor.serializedObject.Update();
                    editor.OnInspectorGUI();
                    editor.serializedObject.ApplyModifiedProperties();
                }

                EditorGUILayout.Space(2);
            }

            if (EditorGUI.EndChangeCheck())
                RepaintViews();

            EditorGUILayout.EndScrollView();
        }

        // -----------------------------------------------------------------------

        private void DrawCameraWarning()
        {
            var mainCam = Camera.main;
            if (mainCam == null)
            {
                EditorGUILayout.HelpBox("No Main Camera found in the scene.", MessageType.Warning);
                return;
            }

            var urpData = mainCam.GetComponent<UniversalAdditionalCameraData>();
            bool postProcessingOn = urpData != null && urpData.renderPostProcessing;

            if (!postProcessingOn)
            {
                EditorGUILayout.HelpBox(
                    "Post Processing is DISABLED on the Main Camera — effects will not be visible.",
                    MessageType.Warning);

                if (GUILayout.Button("Enable Post Processing on Main Camera"))
                {
                    if (urpData == null)
                        urpData = mainCam.gameObject.AddComponent<UniversalAdditionalCameraData>();

                    Undo.RecordObject(urpData, "Enable Post Processing");
                    urpData.renderPostProcessing = true;
                    EditorUtility.SetDirty(urpData);
                    RepaintViews();
                }
            }
        }

        private void DrawProfileField()
        {
            EditorGUI.BeginChangeCheck();
            var newProfile = (VolumeProfile)EditorGUILayout.ObjectField(
                "Volume Profile", _profile, typeof(VolumeProfile), false);

            if (EditorGUI.EndChangeCheck())
            {
                _profile = newProfile;
                ApplyProfileToVolume();
            }
        }

        private void DrawVolumeControls()
        {

            bool volumeExists = _volume != null;
            string label = volumeExists ? "Remove Volume" : "Create Volume";
            Color btnColor = volumeExists ? new Color(1f, 0.4f, 0.4f) : new Color(0.4f, 1f, 0.6f);

            var prevColor = GUI.backgroundColor;
            GUI.backgroundColor = btnColor;

            if (GUILayout.Button(label))
            {
                if (volumeExists)
                    DestroyVolume();
                else
                    CreateVolume();

                RepaintViews();
            }

            GUI.backgroundColor = prevColor;
        }

        // -----------------------------------------------------------------------

        private static void RepaintViews()
        {
            SceneView.RepaintAll();

            if (GameViewType == null) return;
            foreach (var w in Resources.FindObjectsOfTypeAll(GameViewType))
                ((EditorWindow)w).Repaint();
        }
    }
}
