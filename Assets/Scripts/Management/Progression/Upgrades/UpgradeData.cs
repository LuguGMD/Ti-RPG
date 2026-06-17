using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public class UpgradeData : ScriptableObject
    {
        public string upgradeName;
        public string upgradeDescription;
        public int priceUpgrade;
        public string upgradeKey;

        [SerializeReference] public Sprite icon;

        public List<UpgradeData> parents;
    }
}
