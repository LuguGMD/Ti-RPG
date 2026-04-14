using UnityEngine;

namespace RPG.Combat
{
    public class TileObject : MonoBehaviour
    {
        private Tile _currentTile;

        #region Properties

        public Tile CurrentTile { get { return _currentTile; } }

        #endregion

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
            _currentTile = tile;
        }

        public virtual void UpdatePosition()
        {
            transform.position = MapManager.Instance.GetWorldPostion(_currentTile.Position);
        }
    }
}
