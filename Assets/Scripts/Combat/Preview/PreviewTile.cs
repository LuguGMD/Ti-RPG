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
        private Vector2Int _tilePosition;

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

        private void OnEnable()
        {
            ActionsManager.Instance.OnRotationAnimationStarted += RemoveParent;
            ActionsManager.Instance.OnRotationAnimationEnded += AddParent;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnRotationAnimationStarted -= RemoveParent;
            ActionsManager.Instance.OnRotationAnimationEnded -= AddParent;
        }

        public void SetPosition(Vector2Int tilePosition)
        {
            tilePosition = tilePosition.ClampMap();
            _tilePosition = tilePosition;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i)?.gameObject.SetActive(i == _tilePosition.y);
            }

            transform.position = MapManager.Instance.GetWorldPosition(_tilePosition);
            AddParent();
            transform.LookAt(transform.position - (transform.position.normalized));
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }

        private void RemoveParent()
        {
            transform.parent = null;
            transform.GetChild(_tilePosition.y)?.gameObject.SetActive(false);
        }

        private void AddParent()
        {
            transform.parent = MapManager.Map.GetTile(_tilePosition).Transform;
            transform.GetChild(_tilePosition.y)?.gameObject.SetActive(true);
        }

        public void SetMaterial(Material material)
        {
            _renderer[_tilePosition.y].material = material;
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
