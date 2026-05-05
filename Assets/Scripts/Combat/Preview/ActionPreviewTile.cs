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

        public void SetInfo(PreviewTileInfo info)
        {
            _info = info;
        }

        protected override void Select()
        {
            if (_canBeSelected)
                ActionsManager.Instance.OnActionTileSelected?.Invoke(_info);
        }
    }
}
