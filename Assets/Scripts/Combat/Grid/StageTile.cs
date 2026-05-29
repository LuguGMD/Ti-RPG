using RPG.Input;
using UnityEngine;

namespace RPG.Combat.Grid
{
    [RequireComponent(typeof(CursorTarget))]
    public class StageTile : MonoBehaviour
    {
        private CursorTarget _cursorTarget;
        [SerializeField] private Vector2Int _position;
        private Tile _tile;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
        }

        private void Start()
        {
            _cursorTarget.Actions.Hover.OnStart(OnHover);
            _tile = MapManager.Map.GetTile(_position);
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnMapChanged += UpdatePosition;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMapChanged -= UpdatePosition;
        }

        private void OnHover()
        {
            ActionsManager.Instance.OnTileHovered?.Invoke(_position);
        }

        private void UpdatePosition()
        {
            if(_tile != null)
                _position = _tile.Position;
        }
    }
}
