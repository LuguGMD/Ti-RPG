using RPG.Combat.Grid;
using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;
using RPG.Input;

namespace RPG.Combat.Preview
{
    public class PreviewTile : MonoBehaviour
    {
        private PreviewTileInfo _info;
        [SerializeField] private MeshRenderer[] _renderer;
        private bool _canBeSelected = false;
        private int _tileIndex;

        #region Properties

        public PreviewTileInfo Info { get { return _info; } }
        public bool CanBeSelected { get { return _canBeSelected; } }

        #endregion

        private void Start()
        {
            CursorTarget[] cursorTargets = GetComponentsInChildren<CursorTarget>(true); 

            foreach (CursorTarget cursorTarget in cursorTargets)
            {
                cursorTarget.Actions.LeftClick.OnCancel(Select);
            }
        }

        public void SetPosition(Vector2Int tilePosition)
        {
            tilePosition = tilePosition.ClampMap();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i)?.gameObject.SetActive(i == tilePosition.y);
            }

            _tileIndex = tilePosition.y;

            transform.position = MapManager.Instance.GetWorldPostion(tilePosition);
            transform.LookAt(transform.position - (transform.position.normalized));
        }

        public void SetMaterial(Material material)
        {
            _renderer[_tileIndex].material = material;
        }

        public void SetInfo(PreviewTileInfo info)
        {
            _info = info;
        }

        public void SetCanBeSelected(bool canBeSelected)
        {
            _canBeSelected = canBeSelected;
        }

        private void Select()
        {
            if(_canBeSelected)
                ActionsManager.Instance.OnActionTileSelected?.Invoke(_info);
        }
    }
}
