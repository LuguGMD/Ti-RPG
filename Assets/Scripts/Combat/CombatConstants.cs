using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public static class CombatConstants
    {
        public const int MAX_CHARACTERS_COUNT = 3;
        public const float MAX_MOTIVATION_APRESENTADOR = 100f;
        public static readonly Dictionary<CombatTypeEnum, CombatTypeEnum> TypeChart = new Dictionary<CombatTypeEnum, CombatTypeEnum>()
        {
            { CombatTypeEnum.Magic, CombatTypeEnum.Anger },
            { CombatTypeEnum.Strength, CombatTypeEnum.Fear },
            { CombatTypeEnum.Jokes, CombatTypeEnum.Sadness },
            { CombatTypeEnum.Fear, CombatTypeEnum.Magic },
            { CombatTypeEnum.Sadness, CombatTypeEnum.Strength },
            { CombatTypeEnum.Anger, CombatTypeEnum.Jokes },
        };
        public static readonly float[] CombatSpeedTiers = { 1, 2, 3 };
    }
}
