using DG.Tweening;
using Lugu.Singleton;
using RPG.Combat.Grid;
using RPG.Extensions;
using RPG.Management.Progression;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

namespace RPG.Combat
{
    public class SpotlightHandler : SingletonMono<SpotlightHandler>
    {
        private int _lastChangeTurn = 0;
        private int _currentPosition = 0;
        private Vector2Int _currentSpotlightPosition = new Vector2Int(0, 1);
        [SerializeField] private GameObject _visual;
        private const float SPOTLIGHT_MOVE_DURATION = 0.3f;

        private bool _isSuperActive = false;

        #region Properties

        public Vector2Int CurrentSpotlightPosition { get { return _currentSpotlightPosition; } }
        public bool IsSuperActive { get { return _isSuperActive; } }

        #endregion

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnPlayerTurnStarted += CheckChangePosition;
            ActionsManager.Instance.OnSpotlightSuperStarted += HandleSpotlightSuper;
            ActionsManager.Instance.OnTileHovered += OnTileHovered;
            ActionsManager.Instance.OnPreviewTileSelected += OnTileSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnPlayerTurnStarted -= CheckChangePosition;
            ActionsManager.Instance.OnSpotlightSuperStarted -= HandleSpotlightSuper;
            ActionsManager.Instance.OnTileHovered -= OnTileHovered;
            ActionsManager.Instance.OnPreviewTileSelected -= OnTileSelected;
        }

        private void CheckChangePosition()
        {
            if (CombatManager.TurnCount >= _lastChangeTurn + CombatConstants.SPOTLIGHT_CHANGE_TURNS)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);
                int position = chance > 0.5f ? 1 : -1;
                ChangePosition(_currentPosition + (position * (Map.Columns / CombatConstants.MAP_SECTIONS)));
            }
        }

        private void ChangePosition(int position)
        {
            _lastChangeTurn = CombatManager.TurnCount;

            _currentPosition = position;

            _currentPosition += Map.Columns;
            _currentPosition %= Map.Columns;
            StartCoroutine(UpdateSpotlightPosition());
        }

        private IEnumerator UpdateSpotlightPosition()
        {
            Vector2Int previousSpotlightPosition = _currentSpotlightPosition;
            _currentSpotlightPosition = new Vector2Int(_currentPosition, 1);
            ChangeVisualPosition(_currentSpotlightPosition);
            ActionsManager.Instance.OnSpotlightPositionChanged?.Invoke(_currentSpotlightPosition);
            ActionsManager.Instance.OnMapChanged?.Invoke();

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

        private void ChangeVisualPosition(Vector2Int spotlightPosition)
        {
            transform.position = MapManager.Instance.GetWorldPosition(spotlightPosition);
            transform.LookAt(Vector3.zero);
        }

        #region Super

        private void HandleSpotlightSuper()
        {
            _isSuperActive = true;
        }

        private void OnTileHovered(Vector2Int tile)
        {
            if (!_isSuperActive) return;

            Vector2Int position = tile;
            position.y = 1;
            ChangeVisualPosition(position);
        }

        private void OnTileSelected(Vector2Int tile)
        {
            if (!_isSuperActive) return;
            ChangePosition(tile.x);
            _isSuperActive = false;
            ActionsManager.Instance.OnSpotlightSuperEnded?.Invoke();
        }

        #endregion

        private void Init()
        {
            if (CombatManager.HasUpgrade(UpgradeConstants.UpgradeKey.Spotlight))
            {
                _currentPosition = UnityEngine.Random.Range(0, CombatConstants.MAP_SECTIONS);
                _currentPosition *= (Map.Columns / CombatConstants.MAP_SECTIONS);
                _visual.SetActive(false);
                StartCoroutine(UpdateSpotlightPosition());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
