using UnityEngine;
using UnityEditor;

namespace Lugu.UI
{
    [CustomEditor(typeof(PanelsController))]
    public class PanelsControllerEditor : Editor
    {
        private PanelsController _panelController;

        private void OnEnable()
        {
            _panelController = (PanelsController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            for (int i = 0; i < _panelController.panelsList.Count; i++)
            {
                GameObject panel = _panelController.panelsList[i];

                if (panel == null) continue;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(panel.name);
                if (GUILayout.Button("Toggle"))
                {
                    Undo.RecordObjects(_panelController.panelsList.ToArray(), "PanelController Toggle");
                    if (panel.activeSelf)
                    {
                        _panelController.ClosePanel(i);
                    }
                    else
                    {
                        _panelController.OpenPanel(i);
                    }
                }
                if (GUILayout.Button("Choose"))
                {
                    Undo.RecordObjects(_panelController.panelsList.ToArray(), "PanelController ChangePanel");
                    _panelController.ChangePanel(i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
