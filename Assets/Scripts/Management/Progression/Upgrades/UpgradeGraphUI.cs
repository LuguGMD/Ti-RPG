using Lugu.Singleton;
using RPG;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGraphUI : SingletonMono<UpgradeGraphUI>
{
    [Header("Configuração")]
    public UpgradeData[] allUpgrades;          
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
        foreach (var upgrade in allUpgrades)
            depthMap[upgrade] = GetDepth(upgrade, depthMap);

        Dictionary<int, List<UpgradeData>> layers = new();
        foreach (var kvp in depthMap)
        {
            if (!layers.ContainsKey(kvp.Value))
                layers[kvp.Value] = new List<UpgradeData>();
            layers[kvp.Value].Add(kvp.Key);
        }

        foreach (var layer in layers)
        {
            int depth = layer.Key;
            var upgradesInLayer = layer.Value;
            float totalWidth = (upgradesInLayer.Count - 1) * horizontalSpacing;

            for (int i = 0; i < upgradesInLayer.Count; i++)
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

            node.parentNodes = new UpgradeNode[data.parents.Length];
            for (int i = 0; i < data.parents.Length; i++)
            {
                node.parentNodes[i] = nodeMap[data.parents[i]];
                DrawArrow(nodeMap[data.parents[i]], node);
            }

            node.RefreshVisual();
        }
    }

    int GetDepth(UpgradeData upgrade, Dictionary<UpgradeData, int> cache)
    {
        if (cache.ContainsKey(upgrade)) return cache[upgrade];
        if (upgrade.parents == null || upgrade.parents.Length == 0) return 0;

        int max = 0;
        foreach (var parent in upgrade.parents)
            max = Mathf.Max(max, GetDepth(parent, cache));

        return max + 1;
    }

    void DrawArrow(UpgradeNode from, UpgradeNode to)
    {
        var arrow = Instantiate(arrowPrefab, graphContainer);
        arrow.transform.SetAsFirstSibling(); 

        var fromPos = from.GetComponent<RectTransform>().anchoredPosition;
        var toPos = to.GetComponent<RectTransform>().anchoredPosition;  

        var arrowRect = arrow.GetComponent<RectTransform>();
        Vector2 dir = toPos - fromPos;
        float dist = dir.magnitude;

        arrowRect.anchoredPosition = fromPos + dir * 0.5f;
        arrowRect.sizeDelta = new Vector2(dist, 4f);
        arrowRect.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        arrows.Add(arrow);
    }

    public void RefreshAll()
    {
        foreach (var node in nodeMap.Values)
            node.RefreshVisual();
    }
}