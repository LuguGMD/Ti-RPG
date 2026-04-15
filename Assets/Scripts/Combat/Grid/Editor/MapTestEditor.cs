using UnityEngine;
using UnityEditor;

namespace RPG.Combat.Grid.Editor
{
    [CustomEditor(typeof(MapTest))]
    public class MapTestEditor : UnityEditor.Editor
    {
        private MapTest _mapTest;

        private void OnEnable()
        {
            _mapTest = (MapTest)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("Change Row");

            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("<"))
            {
                _mapTest.ChangeRow(-1);
            }

            if (GUILayout.Button(">"))
            {
                _mapTest.ChangeRow(1);
            }


            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Rotate Row");

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("<"))
            {
                _mapTest.Rotate(-1);
            }

            if (GUILayout.Button(">"))
            {
                _mapTest.Rotate(1);
            }


            EditorGUILayout.EndHorizontal();

        }
    }
}
