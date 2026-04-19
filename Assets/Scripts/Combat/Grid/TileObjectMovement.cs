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
        private Queue<Movement> _movementQueue = new Queue<Movement>();
        private int _patternMovementCount = 0;
        private int _movementCount = 0;


        #region Properties

        public TileObject TileObject { get { return _tileObject; } }
        public Queue<Movement> MovementQueue { get { return _movementQueue; } }

        #endregion

        private void Awake()
        {
            _tileObject = GetComponent<TileObject>();
        }

        private void ChangeTile(Direction direction)
        {
            Tile currentTile = _tileObject.CurrentTile;
            Tile nextTile = MapManager.Map.GetNeighborTile(currentTile, direction);
            _tileObject.SetCurrentTile(nextTile);
            _tileObject.UpdatePosition();
        }

        public IEnumerator Move(bool isMirrored)
        {
            Movement movement = _movementQueue.Dequeue();
            Direction direction = isMirrored ? movement.Direction.Mirror() : movement.Direction;
            _tileObject.SetDirection(direction);

            if (MapManager.IsMovementValid(_tileObject.Position, movement, isMirrored))
            {
                ActionsManager.Instance.OnTileStepBefore?.Invoke();

                if(direction.IsSideways())
                {
                    yield return MoveAlongSpline(0.3f, direction);
                }
                else
                {
                    yield return MoveAlongPoints(0.3f, direction);
                }
                

                ChangeTile(direction);

                ActionsManager.Instance.OnTileStepAfter?.Invoke();
            }

            _movementCount++;



            if (_movementCount % _patternMovementCount == 0)
            {
                ActionsManager.Instance.OnPatternEnd?.Invoke();
            }

        }

        private IEnumerator MoveAlongSpline(float time, Direction direction)
        {
            Spline spline = MapManager.Instance.GetCurrentSpline(_tileObject.Position);

            Vector2Int targetTile = _tileObject.Position + direction.ToVector2Int();
            Vector3 targetPos = MapManager.Instance.GetWorldPostion(targetTile);

            float startPercentage = MapManager.Instance.GetCurrentTilePercentage(_tileObject.Position);
            float endPercentage = MapManager.Instance.GetCurrentTilePercentage(targetTile);

            DOVirtual.Float(0f, 1f, time / CombatManager.CombatSpeed, t => {
                float currentPercentage = Mathf.Lerp(startPercentage, endPercentage, t);
                Vector3 position = spline.EvaluatePosition(currentPercentage);
                transform.position = position;
            }).SetEase(Ease.Linear);

            yield return new WaitForSeconds(time / CombatManager.CombatSpeed);
        }

        private IEnumerator MoveAlongPoints(float time, Direction direction)
        {
            Vector2Int targetTile = _tileObject.Position + direction.ToVector2Int();
            Vector3 targetPos = MapManager.Instance.GetWorldPostion(targetTile);

            transform.DOMove(targetPos, time / CombatManager.CombatSpeed).SetEase(Ease.Linear);

            yield return new WaitForSeconds(time / CombatManager.CombatSpeed);
        }

        public void Push(Movement movement)
        {
            if (MapManager.IsMovementValid(_tileObject.Position, movement, false))
            {
                ChangeTile(movement.Direction);
            }
        }

        public void EnqueuePattern(List<Movement> pattern, int repetitions)
        {
            _patternMovementCount = pattern.Count;
            for (int i = 0; i < repetitions; i++)
            {
                for (int j = 0; j < pattern.Count; j++)
                {
                    _movementQueue.Enqueue(pattern[j]);
                }
            }
            _movementCount = 0;
        }

    }
}
