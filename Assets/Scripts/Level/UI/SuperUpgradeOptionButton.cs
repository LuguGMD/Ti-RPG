using RPG.Combat.Upgrades;
using RPG.Management.Progression;
using RPG.Save;
using RPG.UI;
using RPG.UI.Tooltip;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level.UI
{
    public class SuperUpgradeOptionButton : UIButtonHandler
    {
        [SerializeField] private CombatUpgradeScriptable _info;
        private TooltipTrigger _tooltipTrigger;

        protected override void Awake()
        {
            base.Awake();
            _tooltipTrigger = GetComponent<TooltipTrigger>();
        }

        private void Start()
        {
            if (UpgradeConstants.HasUpgrade(_info.UpgradeKey))
            {
                while (_info.NextUpgrade != null && UpgradeConstants.HasUpgrade(_info.NextUpgrade.UpgradeKey))
                {
                    _info = _info.NextUpgrade;
                }

                _tooltipTrigger.content = _info.UpgradeName;

                gameObject.SetActive(true);
                CheckSelected(GameManager.CurrentTileEqquiped);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCurrentSuperUpgradeSet += CheckSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCurrentSuperUpgradeSet -= CheckSelected;
        }

        protected override void OnClick()
        {
            if (GameManager.CurrentSuperEqquiped == _info.UpgradeKey)
            {
                GameManager.Instance.SetCurrentSuper(UpgradeConstants.UpgradeKey.Test);
            }
            else
            {
                GameManager.Instance.SetCurrentSuper(_info.UpgradeKey);
            }
        }

        private void CheckSelected(UpgradeConstants.UpgradeKey upgradeKey)
        {
            _button.GetComponent<Image>().color = _info.UpgradeKey == upgradeKey ? Color.white : Color.gray3;
        }
    }
}
