using Lugu.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatManager : SingletonMono<CombatManager>
    {
        public static readonly Dictionary<CombatType, CombatType> TypeChart = new Dictionary<CombatType, CombatType>()
        {
            { CombatType.Magic, CombatType.Anger },
            { CombatType.Strength, CombatType.Fear },
            { CombatType.Jokes, CombatType.Sadness },
            { CombatType.Fear, CombatType.Magic },
            { CombatType.Sadness, CombatType.Strength },
            { CombatType.Anger, CombatType.Jokes },
        };

        public static void Attack(/*Entity attacker, Entity defender, Effect effect*/)
        {

        }
    }
}
