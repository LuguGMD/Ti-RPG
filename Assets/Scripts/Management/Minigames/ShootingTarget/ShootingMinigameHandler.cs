using RPG.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Management.Minigames.Shooting
{
    public class ShootingMinigameHandler : MonoBehaviour
    {
        [SerializeField] private ShootingTarget _targetPrefab;
        [SerializeField] private List<GameObject> _spawnPositions;

        [SerializeField] private float _spawnCooldown = 1f;

        private int _lastSpawnIndex = -1;

        private void Start()
        {
            CursorInput.Actions.LeftClick.OnStart(HandleShoot);
            StartCoroutine(RunMinigame());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            CursorInput.Actions.LeftClick.Remove.OnStart(HandleShoot);
        }

        private void HandleShoot()
        {
            if (MinigameManager.IsPaused) return;

            Ray ray = CursorInput.Actions.Ray.LastValue;
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, CursorInput.CollisionLayers))
            {
                if (hit.collider.TryGetComponent(out ShootingTarget target))
                {
                    target.TryHit(hit.point);
                }
            }
        }

        private IEnumerator RunMinigame()
        {
            while (true)
            {
                float nextSpawnTime = _spawnCooldown;
                while (nextSpawnTime > 0)
                {
                    if (!MinigameManager.IsPaused)
                    {
                        nextSpawnTime -= Time.deltaTime;
                    }

                    yield return null;
                }

                SpawnTarget();

                yield return null;
            }
        }

        private void SpawnTarget()
        {
            if (_spawnPositions.Count == 0 || _targetPrefab == null) return;

            int index = Random.Range(0, _spawnPositions.Count);
            if (_spawnPositions.Count > 1)
            {
                while (index == _lastSpawnIndex)
                {
                    index = Random.Range(0, _spawnPositions.Count);
                }
            }
            _lastSpawnIndex = index;

            Vector3 spawnPosition = _spawnPositions[index].transform.position;
            Instantiate(_targetPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
