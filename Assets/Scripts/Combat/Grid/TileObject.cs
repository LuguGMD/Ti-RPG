using System;
using UnityEngine;

namespace RPG.Combat.Grid
{
    public class TileObject : MonoBehaviour
    {
        private Tile _currentTile;
        private DirectionEnum _direction;
        protected bool _isOnSpotlight;
        public Action<bool> OnSpotlightStateChange;

        #region Properties

        public Tile CurrentTile { get { return _currentTile; } }
        public Vector2Int Position { get { return _currentTile.Position; }  }
        public DirectionEnum Direction { get { return _direction; } }
        public bool IsOnSpotlight {  get { return _isOnSpotlight; } }

        #endregion

        protected void Awake()
        {
            SetDirection(DirectionEnum.Up);
        }

        private void Start()
        {
            CheckSpotlight();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnMapChanged += UpdatePosition;
            ActionsManager.Instance.OnMapChanged += CheckSpotlight;
            ActionsManager.Instance.OnTurnPassed += CheckSpotlight;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMapChanged -= UpdatePosition;
            ActionsManager.Instance.OnMapChanged -= CheckSpotlight;
            ActionsManager.Instance.OnTurnPassed -= CheckSpotlight;
        }

        public void SetCurrentTile(Tile tile, bool doReplace)
        {
            if(_currentTile != null && _currentTile.TileObject == this)
            {
                _currentTile.SetTileObject(null);
            }

            _currentTile = tile;

            if (doReplace || !_currentTile.IsOccupied)
            {
                _currentTile.SetTileObject(this);
            }
        }

        public void SetDirection(DirectionEnum direction)
        {
            _direction = direction;
        }

        public virtual void UpdatePosition()
        {
            transform.parent = _currentTile.Transform;
            transform.localPosition = Vector3.zero;
            transform.LookAt(transform.position + (transform.position.normalized));
        }

        public void CheckSpotlight()
        {
            if (MapManager.Instance == null || _currentTile == null) return;
            UpdatePosition();

            bool previousState = _isOnSpotlight;
            _isOnSpotlight = MapManager.Instance.IsPositionOnSpotlight(Position);
            if(previousState != _isOnSpotlight)
            {
                OnSpotlightStateChange?.Invoke(_isOnSpotlight);
            }
        }
    }
}
