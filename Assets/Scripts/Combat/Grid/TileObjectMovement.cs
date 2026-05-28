using DG.Tweening;
using RPG.Combat.Actions;
using RPG.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace RPG.Combat.Grid
{
    [RequireComponent(typeof(TileObject))]
    public class TileObjectMovement : MonoBehaviour
    {
        private TileObject _tileObject;


        #region Properties

        public TileObject TileObject { get { return _tileObject; } }

        #endregion

        private void Awake()
        {
            _tileObject = GetComponent<TileObject>();
        }

        private void ChangeTile(DirectionEnum direction, bool doReplace)
        {
            Tile currentTile = _tileObject.CurrentTile;
            Tile nextTile = MapManager.Map.GetNeighborTile(currentTile, direction);

            _tileObject.SetCurrentTile(nextTile, doReplace);
        }

        private void ChangeTile(Vector2Int position, bool doReplace)
        {
            Tile nextTile = MapManager.Map.GetTile(position);

            _tileObject.SetCurrentTile(nextTile, doReplace);
        }

        public IEnumerator Move(Movement movement, int count)
        {
            for (int i = 0; i < count; i++)
            {
                transform.parent = null;
                DirectionEnum direction = movement.Direction;
                _tileObject.SetDirection(direction);

                if (MapManager.IsMovementValid(_tileObject.Position, movement))
                {

                    if (direction.IsSideways())
                    {
                        yield return MoveAlongSpline(0.3f, direction);
                    }
                    else
                    {
                        yield return MoveAlongPoints(0.3f, direction);
                    }

                    ChangeTile(direction, movement.NeedsToBeEmpty);
                }
            }
        }

        public void Teleport(DirectionEnum direction, Vector2Int position)
        {
            transform.parent = null;

            if (MapManager.IsPositionValid(position))
            {
                _tileObject.SetDirection(direction);

                ChangeTile(position, true);
            }
        }

        private IEnumerator MoveAlongSpline(float time, DirectionEnum direction)
        {
            Spline spline = MapManager.Instance.GetCurrentSpline(_tileObject.Position);

            Vector2Int targetTile = _tileObject.Position + direction.ToVector2Int();
            Vector3 targetPos = MapManager.Instance.GetWorldPosition(targetTile);

            float startPercentage = MapManager.Instance.GetCurrentTilePercentage(_tileObject.Position);
            float endPercentage = MapManager.Instance.GetCurrentTilePercentage(targetTile);

            DOVirtual.Float(0f, 1f, time / CombatManager.CombatSpeed, t => {
                float currentPercentage = Mathf.Lerp(startPercentage, endPercentage, t);
                Vector3 position = spline.EvaluatePosition(currentPercentage);
                transform.LookAt(position);
                transform.position = position;
            }).SetEase(Ease.Linear);

            yield return new WaitForSeconds(time / CombatManager.CombatSpeed);
        }

        private IEnumerator MoveAlongPoints(float time, DirectionEnum direction)
        {
            Vector2Int targetTile = _tileObject.Position + direction.ToVector2Int();
            Vector3 targetPos = MapManager.Instance.GetWorldPosition(targetTile);

            transform.LookAt(targetPos);
            transform.DOMove(targetPos, time / CombatManager.CombatSpeed).SetEase(Ease.Linear);

            yield return new WaitForSeconds(time / CombatManager.CombatSpeed);
        }

        public void Push(Movement movement)
        {
            if (MapManager.IsMovementValid(_tileObject.Position, movement))
            {
                ChangeTile(movement.Direction, true);
                _tileObject.UpdatePosition();
            }
        }

    }
}
