using Lugu.Singleton;
using RPG.Combat.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Wave
{
    public class WaveManager : SingletonMono<WaveManager>
    {
        private List<WaveInfo> _spawnedWaves = new List<WaveInfo>();
        private bool _areAllWavesSpawned = false;
        private List<EnemySpawnInfo> _pendingEnemySpawns = new List<EnemySpawnInfo>();

        #region Properties

        public static bool AreAllWavesSpawned { get { return Instance._areAllWavesSpawned; } }

        #endregion

        private void Start()
        {
            CheckWavesToSpawn();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnTurnPassed += CheckWavesToSpawn;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnTurnPassed -= CheckWavesToSpawn;
        }

        private void CheckWavesToSpawn()
        {
            TrySpawnQueuedEnemies();

            WaveInfo[] waves = GameManager.SelectedLevel.Waves;
            foreach (WaveInfo wave in waves)
            {
                if (wave.TurnCount <= CombatManager.TurnCount && !_spawnedWaves.Contains(wave))
                {
                    SpawnWave(wave);
                }
            }

            _areAllWavesSpawned = (waves.Length == _spawnedWaves.Count) && (_pendingEnemySpawns.Count == 0);
        }

        private void SpawnWave(WaveInfo wave)
        {
            foreach (EnemySpawnInfo spawn in wave.Spawns)
            {
                SpawnEnemy(spawn);
            }

            _spawnedWaves.Add(wave);
        }

        private bool SpawnEnemy(EnemySpawnInfo spawn)
        {
            EnemySpawnWarning enemyInstance = CombatFactory.InstantiateEnemyWarning(spawn);
            bool spawnedSuccessfully = enemyInstance != null;

            if (!spawnedSuccessfully && !_pendingEnemySpawns.Contains(spawn))
            {
                _pendingEnemySpawns.Add(spawn);
            }

            return spawnedSuccessfully;
        }

        private void TrySpawnQueuedEnemies()
        {
            for (int i = 0; i < _pendingEnemySpawns.Count; i++)
            {
                bool spawnedSuccessfully = SpawnEnemy(_pendingEnemySpawns[i]);
                if (spawnedSuccessfully)
                {
                    _pendingEnemySpawns.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
