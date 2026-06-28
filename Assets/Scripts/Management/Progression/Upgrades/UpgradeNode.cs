using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using RPG;
using RPG.UI.Tooltip;
using RPG.Management.Progression;

namespace RPG.Management.Progression
{
    public class UpgradeNode : MonoBehaviour
    {
        [Header("Referências UI")]
        public Image iconImage;
        [SerializeField] private TooltipTrigger _tooltip;

        private UpgradeData _data;
        [HideInInspector] public UpgradeNode[] parentNodes;

        private bool isPurchased = false;
        public bool IsPurchased => isPurchased;

        public void SetPurchasedFromSave(bool value)
        {
            isPurchased = value;
        }

        #region Properties

        public UpgradeData Data
        {
            get { return _data; }
        }

        #endregion

        public void Init(UpgradeData data)
        {
            _data = data;
            iconImage.sprite = _data.icon;
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
            if (isPurchased)
            {
                iconImage.color = Color.white;
            }
            else if (IsUnlocked())
            {
                Color unlockedColor = Color.gray6;
                iconImage.color = unlockedColor;
            }
            else
            {
                Color unlockedColor = Color.gray3;
                iconImage.color = unlockedColor;
            }
        }

        public void OnClick()
        {
            if (!IsUnlocked() || !CanBuy()) return;
            UpgradeGraphUI.Instance.OpenConfirmPanel(this);
        }

        public void Purchase()
        {
            if (!CanBuy()) return;

            GameManager.Instance.SpendCoins(_data.priceUpgrade);
            isPurchased = true;
            RefreshVisual();
            UpgradeGraphUI.Instance.RefreshAll();
        }

    }
}