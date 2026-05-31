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
        private ActionPreviewTile _parent;
        private bool _effectPreviewEnabled = false;

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
            _parent = null;
        }

        public void SetInfo(PreviewTileInfo info)
        {
            _info = info;
        }

        public void SeParent(ActionPreviewTile parent)
        {
            _parent = parent;
        }

        protected override void Select()
        {
            if (_canBeSelected)
                ActionsManager.Instance.OnActionTileSelected?.Invoke(_info);
        }

        protected void ShowEffects()
        {
            if (_info == null || _effectPreviewEnabled || !_canBeSelected) return;
            _effectPreviewEnabled = true;

            foreach (Effect effect in _info.Effects)
            {
                _effectPreviewTiles.AddRange(effect.Preview(_tilePosition, _info.Direction));
            }

            if(_parent != null && _info.DoShowParent)
            {
                _parent.ShowEffects();
            }
            
        }

        protected void HideEffects()
        {
            if (!_effectPreviewEnabled) return;

            for (int i =0; i< _effectPreviewTiles.Count; i++)
            {
                ActionPreviewTile preview = _effectPreviewTiles[i];
                if (preview.gameObject.activeSelf)
                    PreviewTilesPool.Pool.Release(preview);
            }

            _effectPreviewTiles.Clear();

            if (_parent != null && _info.DoShowParent)
            {
                _parent.HideEffects();
            }

            _effectPreviewEnabled = false;
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
