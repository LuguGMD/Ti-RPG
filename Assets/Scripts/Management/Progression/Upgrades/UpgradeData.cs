using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "NewUpgrade", menuName = "Scriptable Objects/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        [Header("identificaaação")]
        public string upgradeName;
        [TextArea] public string upgradeDescription;
        public int priceUpgrade;

        [Header("Texture")]
        public Sprite iconUnlocked;
        public Sprite iconLocked;
        public Sprite iconGrayscale;

        [Header("Grafo")]
        public UpgradeData[] parents;
    }
}
