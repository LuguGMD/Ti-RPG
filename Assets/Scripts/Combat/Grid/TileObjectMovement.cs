using RPG.Combat.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Grid
{
    [RequireComponent(typeof(TileObject))]
    public class TileObjectMovement : MonoBehaviour
    {
        private TileObject _tileObject;
        private Queue<Direction> _movementQueue;

        private void Awake()
        {
            _tileObject = GetComponent<TileObject>();
        }

        public void Move()
        {
            Tile currentTile = _tileObject.CurrentTile;
            Tile nextTile = MapManager.Instance.Map.GetNeighborTile(currentTile, _movementQueue.Dequeue());
            _tileObject.SetCurrentTile(nextTile);
            _tileObject.UpdatePosition();
        }
    }
}
