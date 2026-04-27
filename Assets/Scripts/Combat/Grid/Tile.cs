using UnityEngine;

namespace RPG.Combat.Grid
{
    [System.Serializable]
    public class Tile
    {
        private Vector2Int _position;
        private Transform _transform;
        private TileObject _tileObject;

        #region Properties

        public Vector2Int Position { get { return _position; } }
        public Transform Transform { get { return _transform; } }

        public TileObject TileObject { get { return _tileObject; } }

        public bool IsOccupied { get { return _tileObject != null; } }

        #endregion

        public Tile(Vector2Int position, Transform transform)
        {
            _position = position;
            _transform = transform;
        }

        public void SetTileObject(TileObject tileObject)
        {
            _tileObject = tileObject;
        }

        public TileObject RemoveTileObject()
        {
            TileObject tileObject = _tileObject;
            _tileObject = null;
            return TileObject;
        }

        public void SetPosition(Vector2Int position)
        {
            _position = position;
        }


    }
}
