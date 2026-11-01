
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RPG.Combat.Grid;

namespace RPG.Combat
{
    public class SpectatorSpotlightController : MonoBehaviour
    {
        [Header("Spotlight")]
        [SerializeField] private GameObject _spotlightPrefab;
        [SerializeField] private Vector3 _characterOffset = new Vector3(0f, 1f, 0f);
        [SerializeField] private Vector3 _audienceOffset = new Vector3(0f, 1f, 0f);

        [Header("Animation")]
        [SerializeField] private float _growDuration = 0.25f;
        [SerializeField] private float _stayDuration = 0.7f;
        [SerializeField] private float _shrinkDuration = 0.2f;

        [Header("Visual")]
        [SerializeField, Min(0.1f)]
        private float _widthMultiplier = 3f;
        [SerializeField, Min(0.1f)]
        private float _lengthMultiplier = 1.5f;

        [Header("Audience")]
        [SerializeField] private float _minDistanceToSection = 5f;

        private CharacterController _currentAttacker;
        private EnemyController _lastEnemyHit;

        private readonly Dictionary<int, Vector3> _sectionCenters =
            new Dictionary<int, Vector3>();

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterActionUsed += HandleCharacterAction;
            ActionsManager.Instance.OnEnemyDamageTaken += HandleEnemyDamage;
            ActionsManager.Instance.OnActionEnd += HandleActionEnd;
        }

        private void OnDisable()
        {
            if (ActionsManager.Instance == null)
                return;

            ActionsManager.Instance.OnCharacterActionUsed -= HandleCharacterAction;
            ActionsManager.Instance.OnEnemyDamageTaken -= HandleEnemyDamage;
            ActionsManager.Instance.OnActionEnd -= HandleActionEnd;
        }

        private void Start()
        {
            CalculateSectionCenters();
        }

        private void HandleCharacterAction(CharacterController character)
        {
            _currentAttacker = character;
            _lastEnemyHit = null;
        }

        private void HandleEnemyDamage(EnemyController enemy, float amount)
        {
            if (_currentAttacker == null || enemy == null)
                return;

            if (amount >= 0f)
                return;

            _lastEnemyHit = enemy;
        }

        private void HandleActionEnd()
        {
            if (_currentAttacker != null && _lastEnemyHit != null)
            {
                SpawnSpotlight(_currentAttacker, _lastEnemyHit);
            }

            _currentAttacker = null;
            _lastEnemyHit = null;
        }

        private void CalculateSectionCenters()
        {
            _sectionCenters.Clear();

            SpectatorHandler[] spectators =
                GetComponentsInChildren<SpectatorHandler>(true);

            for (int i = 0; i < Map.Columns; i++)
            {
                Vector3 tilePosition = MapManager.Instance.GetWorldPosition(
                    new Vector2Int(i, Map.Rows - 1));

                tilePosition.y = 0f;

                Vector3 totalPosition = Vector3.zero;
                int spectatorCount = 0;

                foreach (SpectatorHandler spectator in spectators)
                {
                    Vector3 spectatorPosition = spectator.transform.position;
                    spectatorPosition.y = 0f;

                    spectatorPosition =
                        spectatorPosition.normalized * tilePosition.magnitude;

                    if (Vector3.Distance(tilePosition, spectatorPosition)
                        < _minDistanceToSection)
                    {
                        totalPosition += spectator.transform.position;
                        spectatorCount++;
                    }
                }

                if (spectatorCount > 0)
                {
                    _sectionCenters[i] = totalPosition / spectatorCount;
                }
                else
                {
                    _sectionCenters[i] = tilePosition;
                }
            }
        }

        private void SpawnSpotlight(
            CharacterController character,
            EnemyController enemy)
        {
            if (_spotlightPrefab == null)
            {
                Debug.LogWarning(
                    "SpectatorSpotlightController: Spotlight Prefab não configurado.");
                return;
            }

            int sectionIndex = enemy.Position.x;

            if (!_sectionCenters.TryGetValue(sectionIndex, out Vector3 target))
            {
                Debug.LogWarning(
                    $"SpectatorSpotlightController: Seção {sectionIndex} não encontrada.");
                return;
            }

            Vector3 origin = character.transform.position + _characterOffset;
            target += _audienceOffset;

            target.y = origin.y;

            Vector3 direction = target - origin;
            float distance = direction.magnitude;

            if (distance <= 0.01f)
                return;

            Quaternion rotation = Quaternion.LookRotation(-direction.normalized);

            Vector3 beamCenter = origin +
            direction.normalized * (distance * _lengthMultiplier * 0.5f);

            GameObject spotlight = Instantiate(
            _spotlightPrefab,
            origin,
            rotation);

            MeshFilter meshFilter =
                spotlight.GetComponentInChildren<MeshFilter>();

            float finalLengthScale = distance;

            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                Transform visual = meshFilter.transform;

                float meshLength =
                    meshFilter.sharedMesh.bounds.size.y *
                    Mathf.Abs(visual.localScale.y);

                if (meshLength > 0.001f)
                    finalLengthScale = distance / meshLength;
            }

            Vector3 finalScale = new Vector3(
                spotlight.transform.localScale.x * _widthMultiplier,
                spotlight.transform.localScale.y * _widthMultiplier,
                finalLengthScale * _lengthMultiplier);

            spotlight.transform.localScale = new Vector3(
                finalScale.x,
                finalScale.y,
                0.001f);

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                spotlight.transform.DOScale(finalScale, _growDuration)
                    .SetEase(Ease.OutCubic));

            sequence.AppendInterval(_stayDuration);

            sequence.Append(
                spotlight.transform.DOScale(
                    new Vector3(finalScale.x, finalScale.y, 0.001f),
                    _shrinkDuration)
                    .SetEase(Ease.InCubic));

            sequence.OnComplete(() => Destroy(spotlight));
        }
    }
}
