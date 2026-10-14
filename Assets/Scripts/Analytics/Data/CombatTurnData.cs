using UnityEngine;

namespace RPG.Analytics
{
    [System.Serializable]
    public class CombatTurnData
    {
        public int TurnNumber;
        public float ApresentadorDamage;
        public float CharactersDamage;
        public float EnemiesDamage;
        public string CharacterDefeated;
        public string EnemyDefeated;
    }
}
