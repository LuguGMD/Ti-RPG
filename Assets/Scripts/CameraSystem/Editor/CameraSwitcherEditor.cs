using UnityEditor;
using UnityEngine;

namespace RPG.CameraSystem.Editor
{
    [CustomEditor(typeof(CameraSwitcher))]
    public class CameraSwitcherEditor : UnityEditor.Editor
    {
        private int displayIndex = 0;
        private string[] cameraNames = new string[] { };

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Seletor de Câmera", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            if (GUILayout.Button("Atualizar Lista de Câmeras"))
            {
                RefreshList();
            }

            EditorGUILayout.Space(5);

            int previousIndex = displayIndex;
            displayIndex = EditorGUILayout.Popup("Câmera", displayIndex, cameraNames);

            if (displayIndex != previousIndex || cameraNames.Length == 0)
            {
                RefreshList();
            }

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Selecionar Câmera"))
            {
                CameraSwitcher switcher = (CameraSwitcher)target;
                switcher.SelectCameraByIndex(displayIndex);
                EditorUtility.SetDirty(switcher);
            }

            if (GUILayout.Button("Trocar para Câmera Selecionada"))
            {
                CameraSwitcher switcher = (CameraSwitcher)target;
                switcher.SwitchToSelectedCamera();
            }

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Listar Câmeras (Console)"))
            {
                CameraSwitcher switcher = (CameraSwitcher)target;
                switcher.ListAllCameras();
            }
        }

        private void RefreshList()
        {
            CameraSwitcher switcher = (CameraSwitcher)target;
            switcher.RefreshAvailableCameras();
            cameraNames = switcher.GetAvailableCameraNames();

            if (displayIndex < 0 || displayIndex >= cameraNames.Length)
                displayIndex = 0;
        }
    }
}
