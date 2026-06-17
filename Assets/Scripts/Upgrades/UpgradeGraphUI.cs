using RPG;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeGraphUI : MonoBehaviour
{
    public static UpgradeGraphUI Instance;

    [Header("Nós fixos (coloquem na ordem de menor para maior)")]
    public UpgradeNode[] nodes; 

    [Header("ConfirmUpgrade Panel")]
    public GameObject confirmPanel;
    public TMP_Text confirmName;
    public TMP_Text confirmDescription;
    public TMP_Text confirmPrice;

    private UpgradeNode pendingNode;

    void Awake() => Instance = this;

    void Start()
    {
        foreach (var node in nodes)
        {
            var parents = node.data.parents;
            node.parentNodes = new UpgradeNode[parents.Length];
            for (int i = 0; i < parents.Length; i++)
            {
                node.parentNodes[i] = FindNodeByData(parents[i]);
            }
            node.RefreshVisual();
        }

        confirmPanel.SetActive(false);
    }

    UpgradeNode FindNodeByData(UpgradeData data)
    {
        foreach (var n in nodes)
            if (n.data == data) return n;
        return null;
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