using RPG.Combat.Upgrades;
using RPG.Management.Progression;
using RPG.UI;
using UnityEngine;

namespace RPG.Level.UI
{
    public class TileUpgradeOptionButton : UIButtonHandler
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
            GameManager.Instance.SetCurrentTile(_info.UpgradeKey);
        }
    }
}
