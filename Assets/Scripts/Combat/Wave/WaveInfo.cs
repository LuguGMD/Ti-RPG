using UnityEngine;

namespace RPG.Combat.Wave
{
    [System.Serializable]
    public class WaveInfo
    {
        //TO DO adicionar sistema de condições; Ex: wave anterior finalizada, inimigos restantes, etc...
        [SerializeField] private int _turnCount;
        [SerializeField] private EnemySpawnInfo[] _spawns;

        #region Properties

        public int TurnCount { get { return _turnCount; } }
        public EnemySpawnInfo[] Spawns { get { return _spawns; } }

        #endregion
    }
}
