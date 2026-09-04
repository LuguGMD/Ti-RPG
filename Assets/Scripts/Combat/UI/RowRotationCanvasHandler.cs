using RPG.Combat.Grid;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Combat.UI
{
    public class RowRotationCanvasHandler : MonoBehaviour
    {
        private Vector2Int _position;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3[] _rowOffset;
        [SerializeField] private Vector3[] _rowScale;
        [SerializeField] private RectTransform _clockwiseButton;
        [SerializeField] private RectTransform _antiClockwiseButton;
        [SerializeField] private ApresentadorUIController _apresentadorUIController;
        private const int COLUMNS_OFFSET = -3;

        private void OnEnable()
        {
            ActionsManager.Instance.OnTileHovered += UpdateRowPosition;
            ActionsManager.Instance.OnRotationAnimationStarted += HandleRowAnimationStart;
            ActionsManager.Instance.OnRotationAnimationEnded += HandleRowAnimationEnd;

            UpdatePosition();
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnTileHovered -= UpdateRowPosition;
            ActionsManager.Instance.OnRotationAnimationStarted -= HandleRowAnimationStart;
            ActionsManager.Instance.OnRotationAnimationEnded -= HandleRowAnimationEnd;
        }

        private void LateUpdate()
        {
            UpdateColumnPosition();
        }

        private void UpdateColumnPosition()
        {
            Vector3 cameraDirection = (Vector3.zero - UnityEngine.Camera.main.transform.position).normalized;
            float anglePerRow = 360f / Map.Columns;
            int columnIndex = Mathf.RoundToInt(Mathf.Atan2(cameraDirection.x, cameraDirection.z) * Mathf.Rad2Deg / anglePerRow);
            columnIndex += COLUMNS_OFFSET;
            columnIndex = (columnIndex + Map.Columns) % Map.Columns;

            if (_position.x == columnIndex) return;
            _position.x = columnIndex;
            UpdatePosition();
        }

        private void UpdateRowPosition(Vector2Int tilePosition)
        {
            if (tilePosition.y < 0 || tilePosition.y >= Map.Rows - 1) return;

            if(_position.y == tilePosition.y) return;
            _position.y = tilePosition.y;

            _apresentadorUIController.ChangeRow(_position.y);
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Tile tile = MapManager.Map.GetTile(_position);
            transform.SetParent(tile.Transform);
            transform.position = tile.Transform.position + _offset;
            transform.LookAt(new Vector3(0, transform.position.y, 0));
            transform.Rotate(90, 0, 0);

            _clockwiseButton.anchoredPosition = _rowOffset[_position.y];
            _clockwiseButton.localScale = _rowScale[_position.y];

            _antiClockwiseButton.anchoredPosition = new Vector2(-_rowOffset[_position.y].x, _rowOffset[_position.y].y);
            _antiClockwiseButton.localScale = _rowScale[_position.y];
        }

        private void HandleRowAnimationStart()
        {
            transform.SetParent(null);
        }

        private void HandleRowAnimationEnd()
        {
            UpdatePosition();
        }
    }
}
