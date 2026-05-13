using RPG.Combat.Grid;
using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;
using RPG.Input;

namespace RPG.Combat.Preview
{
    public class ActionPreviewTile : PreviewTile
    {
        private PreviewTileInfo _info;

        #region Properties

        public PreviewTileInfo Info { get { return _info; } }

        #endregion

        protected new void OnEnable()
        {
            base.OnEnable();
            ActionsManager.Instance.OnPreviewTileSelected += CheckSelected;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            ActionsManager.Instance.OnPreviewTileSelected = CheckSelected;
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

        public void SetMeshes(PreviewTileGroup previewTileGroup)
        {
            if(_info.IsMovement)
                SetMeshes(previewTileGroup.Movement);

            if (_info.IsAttack)
                SetMeshes(previewTileGroup.Effect);
        }

        private void CheckSelected(Vector2Int selectedPosition)
        {
            if(_tilePosition == selectedPosition)
            {
                Select();
            }
        }
    }
}
