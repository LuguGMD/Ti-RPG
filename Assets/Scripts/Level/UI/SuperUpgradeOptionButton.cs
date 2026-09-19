using RPG.Combat.Upgrades;
using RPG.Management.Progression;
using RPG.Save;
using RPG.UI;
using System.Linq;
using UnityEngine;

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
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected override void OnClick()
        {
            GameManager.Instance.SetCurrentSuper(_info.UpgradeKey);
        }
    }
}
