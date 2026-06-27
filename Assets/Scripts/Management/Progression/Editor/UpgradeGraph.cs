using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEditor;
using System;

namespace RPG.Management.Progression.Editor
{
    [Graph(AssetExtension)]
    [Serializable]
    public class UpgradeGraph : Graph
    {
        public const string AssetExtension = "simpleg";

        [MenuItem("Assets/Create/Graph/Upgrade", false)]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<UpgradeGraph>();
        }
    }
}
