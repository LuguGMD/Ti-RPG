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
            UpdatePosition();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCombatStart += DisablePreview;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCombatStart -= DisablePreview;
        }

        private void MoveHoveredTile(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero) return;
            if (!_isEnabled) return;

            _currentHoveredTile += new Vector2Int((int)Mathf.Round(movementInput.x), (int)Mathf.Round(movementInput.y));
            _currentHoveredTile = _currentHoveredTile.ClampMap();

            _currentHoveredTile.y += Map.Rows;
            _currentHoveredTile.y %= Map.Rows;

            UpdatePosition();
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

        
    }
}
