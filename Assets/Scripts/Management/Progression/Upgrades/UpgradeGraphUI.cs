using Lugu.Singleton;
using RPG;
using RPG.Management.Progression;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeGraphUI : SingletonMono<UpgradeGraphUI>
{
    [Header("Configuração")]
    public UpgradeGraphRuntime upgradeGraph;
    public GameObject upgradeNodePrefab;
    public RectTransform graphContainer;
    public GameObject arrowPrefab;

    [Header("Layout")]
    [SerializeField] private float _yOffset = 100f;
    public float horizontalSpacing = 160f;
    public float verticalSpacing = 180f;

    private Dictionary<UpgradeData, UpgradeNode> nodeMap = new();
    private List<GameObject> arrows = new();
    void Start()
    {
        BuildGraph();
    }

    void BuildGraph()
    {

        Dictionary<UpgradeData, int> depthMap = new();
        foreach (var upgrade in upgradeGraph.AllUpgrades)
            depthMap[upgrade] = GetDepth(upgrade, depthMap);

        Dictionary<int, List<UpgradeData>> layers = new();
        foreach (var kvp in depthMap)
        {
            var parents = node.data.parents;
            node.parentNodes = new UpgradeNode[parents.Length];
            for (int i = 0; i < parents.Length; i++)
            {
                var data = upgradesInLayer[i];
                var nodeGO = Instantiate(upgradeNodePrefab, graphContainer);
                var node = nodeGO.GetComponent<UpgradeNode>();
                node.Init(data);

                float x = -totalWidth / 2f + i * horizontalSpacing;
                x += graphContainer.sizeDelta.x / 2;
                float y = depth * verticalSpacing;
                y += _yOffset;
                nodeGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

                nodeMap[data] = node;
            }
        }

        foreach (var kvp in nodeMap)
        {
            var data = kvp.Key;
            var node = kvp.Value;

            node.parentNodes = new UpgradeNode[data.parents.Count];
            for (int i = 0; i < data.parents.Count; i++)
            {
                node.parentNodes[i] = nodeMap[data.parents[i]];
                DrawArrow(nodeMap[data.parents[i]], node);
            }

            node.RefreshVisual();
        }

        confirmPanel.SetActive(false);
    }

    UpgradeNode FindNodeByData(UpgradeData data)
    {
        if (cache.ContainsKey(upgrade)) return cache[upgrade];
        if (upgrade.parents == null || upgrade.parents.Count == 0) return 0;

        int max = 0;
        foreach (var parent in upgrade.parents)
            max = Mathf.Max(max, GetDepth(parent, cache));

        return max + 1;
    }

    public void OpenConfirmPanel(UpgradeNode node)
    {
        pendingNode = node;
        confirmName.text = node.data.upgradeName;
        confirmDescription.text = node.data.upgradeDescription;
        confirmPrice.text = $"Preço: {node.data.priceUpgrade}";
        confirmPanel.SetActive(true);
    }

    public void ConfirmPurchase()
    {
        pendingNode?.Purchase();
        confirmPanel.SetActive(false);
        pendingNode = null;
    }

    public void CancelPurchase()
    {
        confirmPanel.SetActive(false);
        pendingNode = null;
    }

    public void RefreshAll()
    {
        foreach (var node in nodes)
            node.RefreshVisual();
    }
}