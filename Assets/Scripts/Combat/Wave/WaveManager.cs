using Lugu.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Wave
{
    public class WaveManager : SingletonMono<WaveManager>
    {
        private List<WaveInfo> _spawnedWaves = new List<WaveInfo>();
        private bool _areAllWavesSpawned = false;

        #region Properties

        public static bool AreAllWavesSpawned { get { return Instance._areAllWavesSpawned; } }

        #endregion

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
            WaveInfo[] waves = GameManager.SelectedLevel.Waves;
            foreach (WaveInfo wave in waves)
            {
                if (wave.TurnCount <= CombatManager.TurnCount && !_spawnedWaves.Contains(wave))
                {
                    SpawnWave(wave);
                }
            }

            _areAllWavesSpawned = waves.Length == _spawnedWaves.Count;
        }

        private void SpawnWave(WaveInfo wave)
        {
            foreach (EnemySpawnInfo spawn in wave.Spawns)
            {
                CombatFactory.InstantiateEnemy(spawn);
            }

            _spawnedWaves.Add(wave);
        }
    }
}
