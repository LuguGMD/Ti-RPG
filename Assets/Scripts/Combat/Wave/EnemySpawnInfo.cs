using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat.Wave
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        [SerializeField] private EnemyScriptable _enemyInfo;
        [SerializeField] private int _spawnPosition;

        #region Properties

        public EnemyScriptable EnemyInfo { get { return _enemyInfo; } }
        public int SpawnPosition { get { return _spawnPosition; } }

        #endregion
    }
}
