using UnityEditor;
using UnityEngine;

namespace Lugu.BatchRenamer
{
    public class BatchRenamer : EditorWindow
    {
        public string prefix;
        public string rename;
        public string suffix;

        public bool doAddPrefix;
        public bool doRename;
        public bool doAddSuffix;

        public bool doAddNumber;
        public int startNumber;


        private SerializedObject soRenamer;

        private SerializedProperty pPrefix;
        private SerializedProperty pRename;
        private SerializedProperty pSuffix;

        private SerializedProperty pDoAddPrefix;
        private SerializedProperty pDoRename;
        private SerializedProperty pDoAddSuffix;

        private SerializedProperty pDoAddNumber;
        private SerializedProperty pStartNumber;

        [MenuItem("Tools/Lugu/Batch Renamer %#e")]
        public static void OpenWindow()
        {
            EditorWindow window = GetWindow(typeof(BatchRenamer));
            window.titleContent = new GUIContent("Batch Renamer");
        }

        private void OnEnable()
        {
            soRenamer = new SerializedObject(this);

            pPrefix = soRenamer.FindProperty("prefix");
            pRename = soRenamer.FindProperty("rename");
            pSuffix = soRenamer.FindProperty("suffix");

            pDoAddPrefix = soRenamer.FindProperty("doAddPrefix");
            pDoRename = soRenamer.FindProperty("doRename");
            pDoAddSuffix = soRenamer.FindProperty("doAddSuffix");

            pDoAddNumber = soRenamer.FindProperty("doAddNumber");
            pStartNumber = soRenamer.FindProperty("startNumber");

        }

        private void OnGUI()
        {
            soRenamer.Update();

            EditorGUILayout.BeginHorizontal();

            pDoAddPrefix.boolValue = EditorGUILayout.Toggle(doAddPrefix, GUILayout.Width(10));
            GUILayout.Space(5);
            if (!doAddPrefix)
            {
                EditorGUILayout.LabelField("Add Prefix", GUILayout.Width(70));
            }
            if (doAddPrefix)
                pPrefix.stringValue = EditorGUILayout.TextField("Prefix", prefix);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            pDoRename.boolValue = EditorGUILayout.Toggle(doRename, GUILayout.Width(10));
            GUILayout.Space(5);

            if (!doRename)
            {
                EditorGUILayout.LabelField("Rename", GUILayout.Width(70));
            }
            if (doRename)
                pRename.stringValue = EditorGUILayout.TextField("New Name", rename);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            pDoAddSuffix.boolValue = EditorGUILayout.Toggle(doAddSuffix, GUILayout.Width(10));
            GUILayout.Space(5);

            if (!doAddSuffix)
            {
                EditorGUILayout.LabelField("Add Suffix", GUILayout.Width(70));
            }
            if (doAddSuffix)
                pSuffix.stringValue = EditorGUILayout.TextField("Suffix", suffix);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();

            pDoAddNumber.boolValue = EditorGUILayout.Toggle(doAddNumber, GUILayout.Width(10));
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Add Numeric");

            EditorGUILayout.EndHorizontal();

            if (doAddNumber)
                pStartNumber.intValue = EditorGUILayout.IntField("Starting Number", startNumber);

            soRenamer.ApplyModifiedProperties();

            if (GUILayout.Button("Rename Selected"))
            {
                //Starting Changes Group (Undo all at once)
                Undo.SetCurrentGroupName("Renamed Game Objects");
                int group = Undo.GetCurrentGroup();

                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    Undo.RecordObject(Selection.gameObjects[i], "Game_Object_Renamed");

                    if (doRename)
                        Selection.gameObjects[i].name = rename;
                    if (doAddSuffix)
                        Selection.gameObjects[i].name += "_" + suffix;
                    if (doAddPrefix)
                        Selection.gameObjects[i].name = prefix + "_" + Selection.gameObjects[i].name;
                    if (doAddNumber)
                        Selection.gameObjects[i].name += "_" + (i + startNumber).ToString("000");
                }

                //Closing Changes Group
                Undo.CollapseUndoOperations(group);
            }
        }
    }
}