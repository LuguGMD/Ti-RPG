using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using RPG;

public class UpgradeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referências UI")]
    public Image iconImage;
    public GameObject lockIcon;
    public GameObject tooltip;
    public TMP_Text tooltipName;
    public TMP_Text tooltipDescription;
    public TMP_Text tooltipPrice;

    [HideInInspector] public UpgradeData data;
    [HideInInspector] public UpgradeNode[] parentNodes;

    private bool isPurchased = false;
    public bool IsPurchased => isPurchased;

    public bool IsUnlocked()
    {
        foreach (var parent in parentNodes)
            if (!parent.IsPurchased) return false;
        return true;
    }

    public bool CanBuy()
    {
        return IsUnlocked() && !isPurchased && GameManager.Coins >= data.priceUpgrade;
    }

    public void RefreshVisual()
    {
        lockIcon.SetActive(false);

        if (isPurchased)
        {
            iconImage.sprite = data.iconUnlocked;
        }
        else if (IsUnlocked())
        {
            iconImage.sprite = data.iconGrayscale;
        }
        else
        {
            iconImage.sprite = data.iconLocked;
            lockIcon.SetActive(true);
        }
    }

    public void TryPurchase()
    {
        if (!CanBuy()) return;

        GameManager.Instance.SpendCoins(data.priceUpgrade);
        isPurchased = true;
        RefreshVisual();

        UpgradeGraphUI.Instance.RefreshAll();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipName.text = data.upgradeName;
        tooltipDescription.text = data.upgradeDescription;
        tooltipPrice.text = $"Preço: {data.priceUpgrade}";
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}