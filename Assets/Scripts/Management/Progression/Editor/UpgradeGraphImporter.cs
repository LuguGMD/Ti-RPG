using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace RPG.Management.Progression.Editor
{
    [ScriptedImporter(4, UpgradeGraph.AssetExtension)]
    public class UpgradeGraphImporter : ScriptedImporter
    {
        private static Dictionary<UpgradeGraphNode, UpgradeData> _processedNodes;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<UpgradeGraph>(ctx.assetPath);
            if (graph == null)
            {
                Debug.LogError("Failed to load Upgrade Graph asset: " + ctx.assetPath);
            }

            IEnumerable<UpgradeGraphNode> nodes = graph.GetNodes().OfType<UpgradeGraphNode>();
            if (nodes == null) return;

            UpgradeGraphRuntime runtimeAsset = ScriptableObject.CreateInstance<UpgradeGraphRuntime>();
            BuildRuntimeGraph(nodes, runtimeAsset);

            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        static void BuildRuntimeGraph(IEnumerable<UpgradeGraphNode> nodes, UpgradeGraphRuntime runtimeAsset)
        {
            _processedNodes = new Dictionary<UpgradeGraphNode, UpgradeData>();
            foreach (UpgradeGraphNode node in nodes)
            {
                ProcessNode(node, runtimeAsset);
            }
        }

        static void ProcessNode(UpgradeGraphNode node, UpgradeGraphRuntime runtimeAsset)
        {
            UpgradeData upgradeData = GetUpgradeData(node);

            IPort nameInput = node.GetInputPortByName(UpgradeGraphNode.INPUT_NAME);
            nameInput.TryGetValue<string>(out string name);
            upgradeData.upgradeName = name;

            IPort descriptionInput = node.GetInputPortByName(UpgradeGraphNode.INPUT_DESCRIPTION);
            descriptionInput.TryGetValue<string>(out string description);
            upgradeData.upgradeDescription = description;

            IPort priceInput = node.GetInputPortByName(UpgradeGraphNode.INPUT_PRICE);
            priceInput.TryGetValue<int>(out int price);
            upgradeData.priceUpgrade = price;

            IPort iconInput = node.GetInputPortByName(UpgradeGraphNode.INPUT_ICON);
            iconInput.TryGetValue<Sprite>(out Sprite icon);
            upgradeData.icon = icon;


            upgradeData.parents = new List<UpgradeData>();
            INodeOption parentCountOption = node.GetNodeOptionByName(UpgradeGraphNode.PARENT_COUNT_NAME);
            parentCountOption.TryGetValue<int>(out int parentCount);
            for (int i = 0; i < parentCount; i++)
            {
                IPort parentInput = node.GetInputPortByName(UpgradeGraphNode.INPUT_PARENT + i);
                UpgradeGraphNode parentNode = parentInput.firstConnectedPort?.GetNode() as UpgradeGraphNode;

                if (parentNode != null)
                {
                    upgradeData.parents.Add(GetUpgradeData(parentNode));
                }
            }

            runtimeAsset.AllUpgrades.Add(upgradeData);
        }

        static UpgradeData GetUpgradeData(UpgradeGraphNode node)
        {
            UpgradeData upgradeData = null;
            if (_processedNodes.ContainsKey(node))
            {
                upgradeData = _processedNodes[node];
            }
            else
            {
                upgradeData = new UpgradeData();
                _processedNodes.Add(node, upgradeData);
            }

            return upgradeData;
        }
    }
}
