using DG.Tweening;
using Lugu.Singleton;
using RPG.Combat.Actions;
using RPG.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

namespace RPG.Combat.Grid
{
    public class MapManager : SingletonMono<MapManager>
    {
        private Map _map;

        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private SplineContainer _mapSplineContainer;

        [SerializeField] private float _rowRadius = 2f;
        [SerializeField] private float _centerOffset = 3f;

        [SerializeField] private GameObject[] _rowGameObjects;
        private Coroutine _rotationAnimationCoroutine;

        #region Properties

        public static Map Map
        {
            get { return Instance._map; }
        }

        public static GameObject[] RowGameObjects { get { return Instance._rowGameObjects; } }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            if (Instance == this)
            {
                _map = new Map(_rows, _columns);
            }
        }


        [ContextMenu("Generate Map")]
        private void GenerateMap()
        {
            while (_mapSplineContainer.Splines.Count > 0)
            {
                _mapSplineContainer.RemoveSplineAt(0);
            }

            for (int i = 1; i <= _rows; i++)
            {
                Spline spline = SplineFactory.CreateCircle(_centerOffset + _rowRadius * i);
                _mapSplineContainer.AddSpline(spline);
            }
        }

        public void AddTileObject(TileObject tileObject, Vector2Int position)
        {
            Tile tile = _map.GetTile(position);

            tileObject.SetCurrentTile(tile);
            tile.SetTileObject(tileObject);
        }

        public bool IsPositionOccupied(Vector2Int position)
        {
            return _map.GetTile(position).IsOccupied;
        }

        public Vector3 GetWorldPosition(Vector2Int tilePosition)
        {
            Vector3 worldPosition = Vector3.zero;
            if (tilePosition.y >= 0)
            {
                Spline spline = _mapSplineContainer[tilePosition.y];
                float percentage = GetCurrentTilePercentage(tilePosition);
                worldPosition = spline.EvaluatePosition(percentage);
            }
            return worldPosition;
        }

        public static bool IsMovementValid(Vector2Int currentPos, Movement movement, bool isMirrored)
        {
            DirectionEnum movementDirection = isMirrored ? movement.Direction.Mirror() : movement.Direction;
            Vector2Int addedMovement = movementDirection.ToVector2Int();
            Vector2Int finalPos = (currentPos + addedMovement).ClampMap();

            if (finalPos == Map.CENTER_POS || finalPos.y >= Map.Rows - 1)
            {
                return false;
            }

            Tile finalTile = Map.GetTile(finalPos);

            if (movement.NeedsToBeEmpty && finalTile.IsOccupied)
            {
                return false;
            }

            return true;
        }

        public static bool IsPositionValid(Vector2Int tilePosition)
        {
            tilePosition = tilePosition.ClampMap();

            if (tilePosition == Map.CENTER_POS || tilePosition.y >= Map.Rows - 1)
            {
                return false;
            }

            Tile finalTile = Map.GetTile(tilePosition);

            if (finalTile.IsOccupied)
            {
                return false;
            }

            return true;

        }

        public float GetCurrentTilePercentage(Vector2Int tilePosition)
        {
            Spline spline = _mapSplineContainer[tilePosition.y];
            float percentage = (float)tilePosition.x / (float)Map.Columns;
            return percentage;
        }

        public Spline GetCurrentSpline(Vector2Int tilePosition)
        {
            return _mapSplineContainer.Splines[tilePosition.y];
        }

        public void RotateRow(int rowToRotate, int amount)
        {
            _map.RotateRow(rowToRotate, amount);

            if (_rotationAnimationCoroutine != null) StopCoroutine(_rotationAnimationCoroutine);

            _rotationAnimationCoroutine = StartCoroutine(RotateAnimationCoroutine(rowToRotate, amount));
        }

        private IEnumerator RotateAnimationCoroutine(int rowToRotate, int amount)
        {
            ActionsManager.Instance.OnRotationAnimationStarted?.Invoke();

            Transform rowTransform = _rowGameObjects[rowToRotate].transform;
            rowTransform.DOKill(true);

            float currentAngle = rowTransform.localEulerAngles.y;
            Vector3 endValue = Vector3.up * (currentAngle + ((360 / Map.Columns) * amount));
            rowTransform.DORotate(endValue, 0.5f / CombatManager.CombatSpeed, RotateMode.FastBeyond360);

            yield return new WaitForSeconds(0.5f / CombatManager.CombatSpeed);
            ActionsManager.Instance.OnRotationAnimationEnded?.Invoke();
        }
    }
}
