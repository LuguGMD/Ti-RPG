using RPG.Management.Progression;
using UnityEngine;

namespace RPG.Combat.Upgrades
{
    [CreateAssetMenu(fileName = "CombatUpgradeScriptable", menuName = "Scriptable Objects/CombatUpgradeScriptable")]
    public class CombatUpgradeScriptable : ScriptableObject
    {
        [SerializeField] private UpgradeConstants.UpgradeKey _upgradeKey;
        [SerializeField] private string _upgradeName;
        [SerializeField] private string _upgradeDescription;
        [SerializeField] private Sprite _upgradeIcon;
        [SerializeField] private CombatUpgradeScriptable _nextUpgrade;

        #region Properties

        public UpgradeConstants.UpgradeKey UpgradeKey { get { return _upgradeKey; } }
        public string UpgradeName { get { return _upgradeName; } }
        public string UpgradeDescription { get { return _upgradeDescription; } }
        public Sprite UpgradeIcon { get { return _upgradeIcon; } }
        public CombatUpgradeScriptable NextUpgrade { get { return _nextUpgrade; } }

        #endregion
    }
}
