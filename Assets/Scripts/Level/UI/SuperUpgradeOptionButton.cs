using RPG.Combat.Upgrades;
using RPG.Management.Progression;
using RPG.Save;
using RPG.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level.UI
{
    public class SuperUpgradeOptionButton : UIButtonHandler
    {
        [SerializeField] private CombatUpgradeScriptable _info;

        private void Start()
        {
            if (UpgradeConstants.HasUpgrade(_info.UpgradeKey))
            {
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
            GameManager.Instance.SetCurrentSuper(_info.UpgradeKey);
        }

        private void CheckSelected(UpgradeConstants.UpgradeKey upgradeKey)
        {
            _button.GetComponent<Image>().color = _info.UpgradeKey == upgradeKey ? Color.white : Color.gray3;
        }
    }
}
