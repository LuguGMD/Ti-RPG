using RPG.Combat.Actions;
using System.Collections.Generic;
using UnityEngine;

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
            Tile nextTile = MapManager.Instance.Map.GetNeighborTile(currentTile, direction);
            _tileObject.SetCurrentTile(nextTile);
            _tileObject.UpdatePosition();
        }

        public void Move()
        {
            Movement movement = _movementQueue.Dequeue();
            Direction direction = movement.Direction;
            _tileObject.SetDirection(direction);

            if (MapManager.IsMovementValid(_tileObject.Position, movement))
            {
                ActionsManager.Instance.OnTileStepBefore?.Invoke();

                ChangeTile(direction);

                ActionsManager.Instance.OnTileStepAfter?.Invoke();
            }

            _movementCount++;



            if (_movementCount % _patternMovementCount == 0)
            {
                ActionsManager.Instance.OnPatternEnd?.Invoke();
            }

        }

        public void Push(Movement movement)
        {
            if (MapManager.IsMovementValid(_tileObject.Position, movement))
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
