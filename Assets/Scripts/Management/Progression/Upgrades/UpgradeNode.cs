using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using RPG;
using RPG.UI.Tooltip;

public class UpgradeNode : MonoBehaviour
{
    [Header("Referências UI")]
    public Image iconImage;
    public GameObject lockIcon;
    [SerializeField] private TooltipTrigger _tooltip;

    private UpgradeData _data;
    [HideInInspector] public UpgradeNode[] parentNodes;

    private bool isPurchased = false;
    public bool IsPurchased => isPurchased;

    public void Init(UpgradeData data)
    {
        _data = data;
        _tooltip.header = _data.upgradeName;
        _tooltip.content = _data.upgradeDescription + "\nPreço: " + _data.priceUpgrade;

    }

    public bool IsUnlocked()
    {
        foreach (UpgradeNode parent in parentNodes)
            if (!parent.IsPurchased) return false;
        return true;
    }

    public bool CanBuy()
    {
        return IsUnlocked() && !isPurchased && GameManager.Coins >= _data.priceUpgrade;
    }

    public void RefreshVisual()
    {
        lockIcon.SetActive(false);

        if (isPurchased)
        {
            iconImage.sprite = _data.iconUnlocked;
        }
        else if (IsUnlocked())
        {
            iconImage.sprite = _data.iconGrayscale;
        }
        else
        {
            iconImage.sprite = _data.iconLocked;
            lockIcon.SetActive(true);
        }
    }

    public void TryPurchase()
    {
        if (!CanBuy()) return;

        GameManager.Instance.SpendCoins(_data.priceUpgrade);
        isPurchased = true;
        RefreshVisual();

        UpgradeGraphUI.Instance.RefreshAll();
    }

}