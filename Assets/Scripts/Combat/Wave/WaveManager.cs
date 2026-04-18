using Lugu.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Wave
{
    public class WaveManager : SingletonMono<WaveManager>
    {
        private List<WaveInfo> _spawnedWaves = new List<WaveInfo>();

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
                if(wave.TurnCount <= CombatManager.Instance.TurnCount && !_spawnedWaves.Contains(wave))
                {
                    SpawnWave(wave);
                }
            }
        }

        private void SpawnWave(WaveInfo wave)
        {
            foreach(EnemySpawnInfo spawn in wave.Spawns)
            {
                CombatFactory.InstantiateTileObject(spawn);
            }

            _spawnedWaves.Add(wave);
        }
    }
}
