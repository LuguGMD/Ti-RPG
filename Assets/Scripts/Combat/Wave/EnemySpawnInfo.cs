using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat.Wave
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        [SerializeField] private EnemyScriptable _enemyInfo;
        [SerializeField] private int _spawnPosition;
        [SerializeField] private bool _spawnWithSpotlightEffect = false;

        #region Properties

        public EnemyScriptable EnemyInfo { get { return _enemyInfo; } }
        public int SpawnPosition { get { return _spawnPosition; } }
        public bool SpawnWithSpotlightEffect { get { return _spawnWithSpotlightEffect; } }

        #endregion
    }
}
