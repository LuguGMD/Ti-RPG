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
        }

        public static Dictionary<UpgradeKey, string> UpgradeKeys = new Dictionary<UpgradeKey, string>()
        { 
            {UpgradeKey.Test, "UpgradeTest"},
            {UpgradeKey.Spotlight, "UpgradeSpotlight"},
        };
    }
}
