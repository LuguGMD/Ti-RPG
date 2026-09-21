using RPG.Combat.Upgrades;
using RPG.Management.Progression;
using RPG.UI;
using RPG.UI.Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level.UI
{
    public class TileUpgradeOptionButton : UIButtonHandler
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
                while(_info.NextUpgrade != null && UpgradeConstants.HasUpgrade(_info.NextUpgrade.UpgradeKey))
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
            ActionsManager.Instance.OnCurrentTileUpgradeSet += CheckSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCurrentTileUpgradeSet -= CheckSelected;
        }

        protected override void OnClick()
        {
            if(GameManager.CurrentTileEqquiped == _info.UpgradeKey)
            {
                GameManager.Instance.SetCurrentTile(UpgradeConstants.UpgradeKey.Test);
            }
            else
            {
                GameManager.Instance.SetCurrentTile(_info.UpgradeKey);
            }
        }

        private void CheckSelected(UpgradeConstants.UpgradeKey upgradeKey)
        {
            _button.GetComponent<Image>().color = _info.UpgradeKey == upgradeKey ? Color.white : Color.gray3;
        }
    }
}
