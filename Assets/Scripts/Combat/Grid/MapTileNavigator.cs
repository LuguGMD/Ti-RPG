using RPG.Combat.Preview;
using RPG.Extensions;
using RPG.Input;
using UnityEngine;

namespace RPG.Combat.Grid
{
    public class MapTileNavigator : MonoBehaviour
    {
        private Vector2Int _currentHoveredTile = new Vector2Int(0,0);
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] PreviewTile _previewTilePrefab;
        private PreviewTile _previewTile;
        private bool _isEnabled = true;

        private void Start()
        {
            _previewTile = Instantiate<PreviewTile>(_previewTilePrefab);

            _playerInput.Actions.Move.OnUpdate(MoveHoveredTile);
            _playerInput.Actions.Interact.OnStart(SelectHoveredTile);
            UpdatePosition();

            EnablePreview();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnPlayerTurnStarted += EnablePreview;
            //ActionsManager.Instance.OnPlayerTurnEnded += DisablePreview;
            ActionsManager.Instance.OnTileHovered += SetHoveredTile;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnPlayerTurnStarted -= EnablePreview;
            //ActionsManager.Instance.OnPlayerTurnEnded -= DisablePreview;
            ActionsManager.Instance.OnTileHovered -= SetHoveredTile;
        }

        private void SetHoveredTile(Vector2Int hoveredTilePosition)
        {
            _currentHoveredTile = hoveredTilePosition;
            UpdatePosition();
        }

        private void MoveHoveredTile(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero) return;
            if (!_isEnabled) return;

            _currentHoveredTile += new Vector2Int((int)Mathf.Round(movementInput.x), (int)Mathf.Round(movementInput.y));
            _currentHoveredTile = _currentHoveredTile.ClampMap();

            _currentHoveredTile.y += Map.Rows;
            _currentHoveredTile.y %= Map.Rows;

            ActionsManager.Instance.OnTileHovered?.Invoke(_currentHoveredTile);
        }

        private void UpdatePosition()
        {
            _previewTile.SetPosition(_currentHoveredTile);
        }

        private void EnablePreview()
        {
            _isEnabled = true;
            _previewTile.gameObject.SetActive(true);
        }

        private void DisablePreview()
        {
            _isEnabled = false;
            _previewTile.gameObject.SetActive(false);
        }

        private void SelectHoveredTile()
        {
            if (!_isEnabled) return;
            ActionsManager.Instance.OnPreviewTileSelected?.Invoke(_currentHoveredTile);
        }

        
    }
}
