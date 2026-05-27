using RPG.Combat.Grid;
using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;
using RPG.Input;
using RPG.Combat.Actions;

namespace RPG.Combat.Preview
{
    public class ActionPreviewTile : PreviewTile
    {
        private PreviewTileInfo _info;
        private List<ActionPreviewTile> _effectPreviewTiles = new List<ActionPreviewTile>();

        #region Properties

        public PreviewTileInfo Info { get { return _info; } }

        #endregion

        protected new void OnEnable()
        {
            base.OnEnable();
            ActionsManager.Instance.OnPreviewTileSelected += CheckSelected;
            ActionsManager.Instance.OnTileHovered += CheckHovered;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            ActionsManager.Instance.OnPreviewTileSelected -= CheckSelected;
            ActionsManager.Instance.OnTileHovered -= CheckHovered;

            HideEffects();
        }

        public void SetInfo(PreviewTileInfo info)
        {
            _info = info;
        }

        protected override void Select()
        {
            if (_canBeSelected)
                ActionsManager.Instance.OnActionTileSelected?.Invoke(_info);
        }

        protected void ShowEffects()
        {
            if (_info == null) return;

            foreach(Effect effect in _info.Effects)
            {
                _effectPreviewTiles.AddRange(effect.Preview(_tilePosition, _info.Direction));
            }
        }

        protected void HideEffects()
        {
            foreach(ActionPreviewTile preview in _effectPreviewTiles)
            {
                PreviewTilesPool.Pool.Release(preview);
            }
            _effectPreviewTiles.Clear();
        }

        private void CheckSelected(Vector2Int selectedPosition)
        {
            if(_tilePosition == selectedPosition)
            {
                Select();
            }
        }

        private void CheckHovered(Vector2Int hoveredPosition)
        {
            if (_tilePosition == hoveredPosition)
            {
                ShowEffects();
            }
            else
            {
                HideEffects();
            }
        }
    }
}
