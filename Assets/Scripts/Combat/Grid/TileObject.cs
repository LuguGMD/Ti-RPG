using UnityEngine;

namespace RPG.Combat.Grid
{
    public class TileObject : MonoBehaviour
    {
        private Tile _currentTile;
        private DirectionEnum _direction;

        #region Properties

        public Tile CurrentTile { get { return _currentTile; } }
        public Vector2Int Position { get { return _currentTile.Position; }  }
        public DirectionEnum Direction { get { return _direction; } }

        #endregion

        protected void Awake()
        {
            SetDirection(DirectionEnum.Up);
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnMapChanged += UpdatePosition;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMapChanged -= UpdatePosition;
        }

        public void SetCurrentTile(Tile tile)
        {
            if(_currentTile != null)
            {
                _currentTile.SetTileObject(null);
            }

            _currentTile = tile;
            _currentTile.SetTileObject(this);
        }

        public void SetDirection(DirectionEnum direction)
        {
            _direction = direction;
        }

        public virtual void UpdatePosition()
        {
            transform.position = MapManager.Instance.GetWorldPostion(_currentTile.Position);
            transform.LookAt(transform.position + (transform.position.normalized));
            //TO DO look at _direction
        }
    }
}
