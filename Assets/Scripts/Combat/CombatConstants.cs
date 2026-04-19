using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public static class CombatConstants
    {
        public const int MAX_CHARACTERS_COUNT = 3;
        public static readonly Dictionary<CombatType, CombatType> TypeChart = new Dictionary<CombatType, CombatType>()
        {
            { CombatType.Magic, CombatType.Anger },
            { CombatType.Strength, CombatType.Fear },
            { CombatType.Jokes, CombatType.Sadness },
            { CombatType.Fear, CombatType.Magic },
            { CombatType.Sadness, CombatType.Strength },
            { CombatType.Anger, CombatType.Jokes },
        };
        public static readonly float[] CombatSpeedTiers = { 1, 2, 4, 8 };
    }
}
