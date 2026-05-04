using RPG.Combat.Grid;
using System;
using UnityEngine;

namespace RPG.Combat.Wave
{
    public class EnemySpawnWarning : MonoBehaviour
    {
        private EnemySpawnInfo _spawnInfo;
        private const float SPAWN_DISTANCE = 2f;

        private void OnEnable()
        {
            ActionsManager.Instance.OnEnemyTurnEnded += Spawn;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnEnemyTurnEnded -= Spawn;
        }

        public void SetSpawnInfo(EnemySpawnInfo spawnInfo)
        {
            _spawnInfo = spawnInfo;

            Vector2Int spawnTilePos = new Vector2Int(spawnInfo.SpawnPosition, Map.Rows - 1);
            Vector3 spawnWorldPos = MapManager.Instance.GetWorldPosition(spawnTilePos);

            spawnWorldPos += spawnWorldPos.normalized * SPAWN_DISTANCE;

            transform.position = spawnWorldPos;
            transform.LookAt(Vector3.zero);

        }

        private void Spawn()
        {
            EnemyController enemyInstance = CombatFactory.InstantiateEnemy(_spawnInfo);
            if(enemyInstance != null)
            {
                OnSuccessfulSpawn();
            }
        }

        private void OnSuccessfulSpawn()
        {
            Destroy(gameObject);
        }
    }
}
