using RPG.Combat.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Grid
{
    [RequireComponent(typeof(TileObject))]
    public class TileObjectMovement : MonoBehaviour
    {
        private TileObject _tileObject;
        private Queue<Direction> _movementQueue = new Queue<Direction>();
        private int _patternMovementCount = 0;
        private int _movementCount = 0;
        private Direction _direction;

        #region Properties

        public TileObject TileObject { get { return _tileObject; } }
        public Queue<Direction> MovementQueue { get { return _movementQueue; } }
        public Direction Direction { get { return _direction; } }

        #endregion

        private void Awake()
        {
            _tileObject = GetComponent<TileObject>();
        }

        public void Move()
        {
            Direction direction = _movementQueue.Dequeue();
            _direction = direction;

            ActionsManager.Instance.OnTileStepBefore?.Invoke();

            Tile currentTile = _tileObject.CurrentTile;
            Tile nextTile = MapManager.Instance.Map.GetNeighborTile(currentTile, direction);
            _tileObject.SetCurrentTile(nextTile);
            _tileObject.UpdatePosition();

            _movementCount++;

            ActionsManager.Instance.OnTileStepAfter?.Invoke();

            if (_movementCount % _patternMovementCount == 0)
            {
                ActionsManager.Instance.OnPatternEnd?.Invoke();
            }

        }

        public void EnqueuePattern(List<Movement> pattern, int repetitions)
        {
            _patternMovementCount = pattern.Count;
            for (int i = 0; i < repetitions; i++)
            {
                for (int j = 0; j < pattern.Count; j++)
                {
                    _movementQueue.Enqueue(pattern[j].Direction);
                }
            }
            _movementCount = 0;
        }

    }
}
