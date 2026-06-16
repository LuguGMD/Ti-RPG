using RPG.Management.Progression.Editor;
using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    [Graph(AssetExtension)]
    [Serializable]
    public class DialogueGraph : Graph
    {
        public const string AssetExtension = "dlg";

        [MenuItem("Assets/Create/Graph/Dialogue", false)]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
        }
    }
}
