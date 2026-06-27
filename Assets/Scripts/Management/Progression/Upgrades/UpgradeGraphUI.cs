using Lugu.Singleton;
using RPG.Save;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Management.Progression
{

    public class UpgradeGraphUI : SingletonMono<UpgradeGraphUI>, ISavable<UpgradeGraphUI, UpgradeGraphData, UpgradeGraphAdapter>
    {
        [Header("Configuração")]
        public UpgradeGraphRuntime upgradeGraph;
        public GameObject upgradeNodePrefab;
        public RectTransform graphContainer;
        public GameObject arrowPrefab;
        [SerializeField] private UpgradeConfirmPanel confirmPanel;

        [Header("Layout")]
        [SerializeField] private float _yOffset = 100f;
        public float horizontalSpacing = 160f;
        public float verticalSpacing = 180f;

        private Dictionary<UpgradeData, UpgradeNode> nodeMap = new();
        private List<GameObject> arrows = new();
        private UpgradeNode pendingNode;

        private string _key = "UpgradeGraph";
        public string Key { get { return _key; } set { _key = value; } }

        void Start()
        {
            BuildGraph();
            Load();
        }

        void BuildGraph()
        {

            Dictionary<UpgradeData, int> depthMap = new();
            foreach (var upgrade in upgradeGraph.AllUpgrades)
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

                node.parentNodes = new UpgradeNode[data.parents.Count];
                for (int i = 0; i < data.parents.Count; i++)
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
            if (upgrade.parents == null || upgrade.parents.Count == 0) return 0;

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

        public void OpenConfirmPanel(UpgradeNode node)
        {
            pendingNode = node;
            confirmPanel.Init(node);
            confirmPanel.gameObject.SetActive(true);
        }

        public void ConfirmPurchase()
        {
            pendingNode?.Purchase();
            confirmPanel.gameObject.SetActive(false);
            pendingNode = null;

            SaveManager.Instance.SaveAll();
        }

        public void CancelPurchase()
        {
            confirmPanel.gameObject.SetActive(false);
            pendingNode = null;
        }

        public void RefreshAll()
        {
            foreach (var node in nodeMap.Values)
                node.RefreshVisual();
        }

        public List<string> GetPurchasedUpgradeIDs()
        {
            List<string> ids = new();
            foreach (var kvp in nodeMap)
                if (kvp.Value.IsPurchased)
                    ids.Add(GetUpgradeID(kvp.Key));
            return ids;
        }

        public void ApplyPurchasedUpgradeIDs(List<string> ids)
        {
            if (ids == null) return;

            HashSet<string> purchased = new(ids);
            foreach (var kvp in nodeMap)
                kvp.Value.SetPurchasedFromSave(purchased.Contains(GetUpgradeID(kvp.Key)));

            RefreshAll();
        }

        private string GetUpgradeID(UpgradeData data)
        {
            return string.IsNullOrEmpty(data.upgradeKey) ? data.name : data.upgradeKey;
        }

        public void Save()
        {
            ISavable<UpgradeGraphUI, UpgradeGraphData, UpgradeGraphAdapter> savable = (ISavable<UpgradeGraphUI, UpgradeGraphData, UpgradeGraphAdapter>)this;
            savable.SaveInfo();
        }

        public void Load()
        {
            ISavable<UpgradeGraphUI, UpgradeGraphData, UpgradeGraphAdapter> savable = (ISavable<UpgradeGraphUI, UpgradeGraphData, UpgradeGraphAdapter>)this;
            savable.LoadInfo();
        }
    }

}