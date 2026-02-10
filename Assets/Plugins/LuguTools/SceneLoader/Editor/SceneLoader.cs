
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lugu.SceneLoader
{
    public class SceneLoader : EditorWindow
    {
        public SceneAsset[] scenes = new SceneAsset[0];
        public SceneAsset sceneSelected;

        public string[] scenePaths = new string[0];

        private SerializedObject so;
        private SerializedProperty propScenes;
        private SerializedProperty propSceneSelected;
        private SerializedProperty propScenePaths;
        private SerializedProperty propLastScene;

        public SceneAsset _lastScene;
        private bool _playedRemotely = false;

        private enum State
        {
            Use,
            Edit
        }

        private State _currentState = State.Use;

        private Vector2 _scrollViewPos = Vector2.zero;

        [MenuItem("Tools/Lugu/Scene Loader")]
        public static void OpenWindow()
        {
            EditorWindow.GetWindow<SceneLoader>();
        }

        private void OnEnable()
        {
            so = new SerializedObject(this);
            propScenes = so.FindProperty("scenes");
            propSceneSelected = so.FindProperty("sceneSelected");
            propScenePaths = so.FindProperty("scenePaths");
            propLastScene = so.FindProperty("_lastScene");

            Load();

            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
        }

        private void OnDisable()
        {
            Save();

            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
        }

        private void HandlePlayModeStateChanged(PlayModeStateChange playMode)
        {
            switch(playMode)
            {
                case PlayModeStateChange.EnteredEditMode:
                    if (_playedRemotely)
                    {
                        LoadScene(_lastScene);
                    }

                    _playedRemotely = false;
                    break;
                case PlayModeStateChange.ExitingEditMode:

                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
            }
        }

        private void OnGUI()
        {
            object[] objects = EditorUtils.DropZone();

            ClearNullScenes();

            if (DragAndDrop.objectReferences.Length > 0 || scenes?.Length == 0)
            {
                GUILayout.Box("Drag & Drop Scenes Here", GUILayout.Width((int)position.width - 20), GUILayout.Height(50));
            }

            if (objects != null)
                foreach (object obj in objects)
                {
                    if (obj is SceneAsset)
                    {
                        SceneAsset sceneAsset = (SceneAsset)obj;
                        so.Update();
                        propSceneSelected.objectReferenceValue = sceneAsset;
                        so.ApplyModifiedProperties();
                        AddSelectedScene();
                    }
                }

            GUIStyle style = EditorStyles.textField;

            EditorGUILayout.BeginVertical(style);

            switch (_currentState)
            {
                case State.Use:
                    if (GUILayout.Button("Enable Edit Mode"))
                    {
                        _currentState = State.Edit;
                    }
                    break;
                case State.Edit:
                    if (GUILayout.Button("Disable Edit Mode"))
                    {
                        _currentState = State.Use;
                    }
                    break;
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            _scrollViewPos = EditorGUILayout.BeginScrollView(_scrollViewPos, false, false, GUILayout.ExpandHeight(true));

            if (scenes != null)
            {
                for (int i = 0; i < scenes.Length; i++)
                {
                    SceneAsset currentScene = scenes[i];

                    if (currentScene != null)
                    {
                        string sceneName = currentScene.name;

                        EditorGUILayout.BeginHorizontal();

                        if (_currentState == State.Edit)
                        {
                            so.Update();
                            if (GUILayout.Button("↓", GUILayout.Width(20)))
                            {
                                if (i != scenes.Length - 1)
                                    propScenes.MoveArrayElement(i, i + 1);
                            }

                            if (GUILayout.Button("↑", GUILayout.Width(20)))
                            {
                                if (i != 0)
                                    propScenes.MoveArrayElement(i, i - 1);
                            }
                            so.ApplyModifiedProperties();
                        }

                        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

                        if (sceneName == EditorSceneManager.GetActiveScene().name)
                        {
                            buttonStyle.fontStyle = FontStyle.Bold;
                            buttonStyle.fontSize += 1;
                        }

                        if (GUILayout.Button(sceneName, buttonStyle))
                        {
                            LoadScene(i);
                        }

                        if (_currentState == State.Edit)
                        {

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RemoveArrayElementAt(i);
                            }

                        }
                        else if (!Application.isPlaying)
                        {
                            if (GUILayout.Button("Play", GUILayout.Width(60)))
                            {

                                SceneAsset currentLoadedScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorSceneManager.GetActiveScene().path);

                                so.Update();
                                propLastScene.objectReferenceValue = currentLoadedScene;
                                so.ApplyModifiedProperties();

                                _playedRemotely = true;
                                LoadScene(i, true);
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void ClearNullScenes()
        {
            so.Update();
            for (int i = propScenes.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty pScene = propScenes.GetArrayElementAtIndex(i);
                if (pScene.objectReferenceValue == null)
                {
                    propScenes.DeleteArrayElementAtIndex(i);
                    propScenePaths.DeleteArrayElementAtIndex(i);
                }
            }
            so.ApplyModifiedProperties();
        }

        private void RemoveArrayElementAt(int index)
        {
            so.Update();
            propScenes.DeleteArrayElementAtIndex(index);
            propScenePaths.DeleteArrayElementAtIndex(index);
            so.ApplyModifiedProperties();
        }

        private void AddSelectedScene()
        {
            if (!scenes.Contains(sceneSelected))
            {
                so.Update();
                propScenes.InsertArrayElementAtIndex(propScenes.arraySize);
                so.ApplyModifiedProperties();
                so.Update();
                SerializedProperty pScene = propScenes.GetArrayElementAtIndex(propScenes.arraySize - 1);
                pScene.objectReferenceValue = propSceneSelected.objectReferenceValue;
                so.ApplyModifiedProperties();

                so.Update();
                propScenePaths.InsertArrayElementAtIndex(propScenePaths.arraySize);
                SerializedProperty path = propScenePaths.GetArrayElementAtIndex(propScenePaths.arraySize - 1);
                path.stringValue = AssetDatabase.GetAssetPath(sceneSelected);
                so.ApplyModifiedProperties();
            }
        }

        public int SearchScene(string sceneName)
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].name == sceneName)
                {
                    return i;
                }
            }

            return -1;
        }

        private void LoadScene(int index, bool play = false)
        {
            if (!Application.isPlaying)
            {
                CheckForUnsavedScenes();

                string scenePath = AssetDatabase.GetAssetPath(scenes[index]);

                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                if (play)
                {
                    EditorApplication.isPlaying = true;
                }

            }
            else
            {
                SceneManager.LoadScene(scenes[index].name);
            }
        }

        private void LoadScene(SceneAsset scene)
        {
            if (scene != null)
            {
                string scenePath = AssetDatabase.GetAssetPath(scene);
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }
        }

        private bool CheckForUnsavedScenes()
        {
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                bool save = EditorUtility.DisplayDialog("Unsaved Changes", "You have unsaved changes in the current scene. Do you want to save before loading a new scene?", "Save", "Don't Save");
                if (save)
                {
                    EditorSceneManager.SaveOpenScenes();
                }
                return true;
            }
            return false;
        }

        public void Load()
        {
            if (propScenes.arraySize == 0)
            {
                string path = Application.persistentDataPath + "/lugu_scene_loader_save.json";

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    SceneLoaderData data = JsonUtility.FromJson<SceneLoaderData>(json);

                    if (data.scenePaths != null)
                        for (int i = 0; i < data.scenePaths.Length; i++)
                        {
                            SceneAsset sceneToAdd = AssetDatabase.LoadAssetAtPath(data.scenePaths[i], typeof(SceneAsset)) as SceneAsset;
                            so.Update();
                            propScenes.InsertArrayElementAtIndex(i);
                            SerializedProperty pScene = propScenes.GetArrayElementAtIndex(i);
                            pScene.objectReferenceValue = sceneToAdd;

                            so.ApplyModifiedProperties();

                            so.Update();
                            propScenePaths.InsertArrayElementAtIndex(i);
                            SerializedProperty propPath = propScenePaths.GetArrayElementAtIndex(i);
                            propPath.stringValue = AssetDatabase.GetAssetPath(sceneSelected);
                            so.ApplyModifiedProperties();
                        }
                }
            }
        }


        public void Save()
        {
            if (!Application.isPlaying)
            {
                string path = Application.persistentDataPath + "/lugu_scene_loader_save.json";

                SceneLoaderData data = new SceneLoaderData();
                data.scenePaths = new string[scenes.Length];

                for (int i = 0; i < propScenePaths.arraySize; i++)
                {
                    data.scenePaths[i] = propScenePaths.GetArrayElementAtIndex(i).stringValue;
                }

                File.WriteAllText(path, JsonUtility.ToJson(data));
            }
        }
    }
}
