using DG.Tweening;
using RPG.Combat.Grid;
using RPG.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

namespace RPG.Combat
{
    public class SpotlightHandler : MonoBehaviour
    {
        private int _lastChangeTurn = 0;
        private int _currentPosition = 0;
        private Vector2Int _currentSpotlightPosition = new Vector2Int(0, 1);
        [SerializeField] private GameObject _visual;
        private const float SPOTLIGHT_MOVE_DURATION = 0.3f;

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnPlayerTurnStarted += CheckChangePosition;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnPlayerTurnStarted -= CheckChangePosition;
        }

        private void CheckChangePosition()
        {
            if(CombatManager.TurnCount >= _lastChangeTurn + CombatConstants.SPOTLIGHT_CHANGE_TURNS)
            {
                ChangePosition();
            }
        }

        private void ChangePosition()
        {
            _lastChangeTurn = CombatManager.TurnCount;

            float chance = UnityEngine.Random.Range(0f, 1f);
            _currentPosition += chance > 0.5f ? 1 : -1;

            _currentPosition += CombatConstants.MAP_SECTIONS;
            _currentPosition %= CombatConstants.MAP_SECTIONS;
            StartCoroutine(UpdateSpotlightPosition());
        }

        private IEnumerator UpdateSpotlightPosition()
        {
            Vector2Int previousSpotlightPosition = _currentSpotlightPosition;
            _currentSpotlightPosition = new Vector2Int(_currentPosition * (Map.Columns/CombatConstants.MAP_SECTIONS), 1);
            transform.position = MapManager.Instance.GetWorldPosition(_currentSpotlightPosition);
            ActionsManager.Instance.OnSpotlightPositionChanged?.Invoke(_currentSpotlightPosition);
            ActionsManager.Instance.OnMapChanged?.Invoke();

            transform.LookAt(Vector3.zero);

            /*Spline spline = MapManager.Instance.GetCurrentSpline(previousSpotlightPosition);

            Vector2Int targetTile = _currentSpotlightPosition;
            Vector3 targetPos = MapManager.Instance.GetWorldPosition(targetTile);

            float startPercentage = MapManager.Instance.GetCurrentTilePercentage(previousSpotlightPosition);
            float endPercentage = MapManager.Instance.GetCurrentTilePercentage(targetTile);

            DOVirtual.Float(0f, 1f, SPOTLIGHT_MOVE_DURATION / CombatManager.CombatSpeed, t => {
                float currentPercentage = Mathf.Lerp(startPercentage, endPercentage, t);
                Vector3 position = spline.EvaluatePosition(currentPercentage);
                transform.position = position;
            }).SetEase(Ease.Linear);*/

            yield return new WaitForSeconds(SPOTLIGHT_MOVE_DURATION / CombatManager.CombatSpeed);

            _visual.SetActive(true);
        }

        private void Init()
        {
            _currentPosition = UnityEngine.Random.Range(0, CombatConstants.MAP_SECTIONS);
            _visual.SetActive(false);
            StartCoroutine(UpdateSpotlightPosition());
        }
    }
}
