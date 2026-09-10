using System.Collections.Generic;
using UnityEngine;

namespace RPG.Management.Progression
{
    public static class UpgradeConstants
    {
        public enum UpgradeKey
        {
            Test,
            Spotlight,
            SuperSpotlight1,
            SuperSpotlight2,
            SuperSpotlight3,
            SuperHeal1,
            SuperHeal2,
            SuperHeal3,
            SuperPush1,
            SuperPush2,
            SuperPush3,
            TileDamageReduction1,
            TileDamageReduction2,
            TileDamageIncrease1,
            TileDamageIncrease2,
            TilePushBlock1,
            TilePushBlock2,
        }

        public static Dictionary<UpgradeKey, string> UpgradeKeys = new Dictionary<UpgradeKey, string>()
        { 
            {UpgradeKey.Test, "UpgradeTest"},
            {UpgradeKey.Spotlight, "UpgradeSpotlight"},
            {UpgradeKey.SuperSpotlight1, "UpgradeSuperSpotlight1"},
            {UpgradeKey.SuperSpotlight2, "UpgradeSuperSpotlight2"},
            {UpgradeKey.SuperSpotlight3, "UpgradeSuperSpotlight3"},
            {UpgradeKey.SuperHeal1, "UpgradeSuperHeal1"},
            {UpgradeKey.SuperHeal2, "UpgradeSuperHeal2"},
            {UpgradeKey.SuperHeal3, "UpgradeSuperHeal3"},
            {UpgradeKey.SuperPush1, "UpgradeSuperPush1"},
            {UpgradeKey.SuperPush2, "UpgradeSuperPush2"},
            {UpgradeKey.SuperPush3, "UpgradeSuperPush3"},
            {UpgradeKey.TileDamageReduction1, "UpgradeTileDamageReduction1"},
            {UpgradeKey.TileDamageReduction2, "UpgradeTileDamageReduction2"},
            {UpgradeKey.TileDamageIncrease1, "UpgradeTileDamageIncrease1"},
            {UpgradeKey.TileDamageIncrease2, "UpgradeTileDamageIncrease2"},
            {UpgradeKey.TilePushBlock1, "UpgradeTilePushBlock1"},
            {UpgradeKey.TilePushBlock2, "UpgradeTilePushBlock2"},
        };
    }
}
