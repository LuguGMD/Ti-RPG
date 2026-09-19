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

        [SerializeField] private UpgradeData _data;
        [HideInInspector] public UpgradeNode[] parentNodes;

        [SerializeField] private Image _lockImage;
        [SerializeField] private Sprite _closedLockIcon;
        [SerializeField] private Sprite _openLockIcon;

        private Button _button;

        private bool isPurchased = false;
        public bool IsPurchased => isPurchased;


        #region Properties

        public UpgradeData Data
        {
            get { return _data; }
        }

        #endregion

        public void SetPurchasedFromSave(bool value)
        {
            isPurchased = value;
        }

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
            if (_button == null) _button = GetComponent<Button>();

            if (isPurchased)
            {
                iconImage.color = Color.white;

                _lockImage.enabled = false;
                _button.interactable = true;
            }
            else if (IsUnlocked())
            {
                Color unlockedColor = Color.gray5;
                iconImage.color = unlockedColor;
                _lockImage.enabled = false;

                _button.interactable = true;
            }
            else
            {
                Color unlockedColor = Color.gray2;
                iconImage.color = unlockedColor;

                _button.interactable = false;

                _lockImage.sprite = _closedLockIcon;
                _lockImage.enabled = true;
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