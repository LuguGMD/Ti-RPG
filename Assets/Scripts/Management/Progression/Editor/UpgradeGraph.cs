using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEditor;
using System;

namespace RPG.Management.Progression.Editor
{
    [Graph(AssetExtension)]
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
