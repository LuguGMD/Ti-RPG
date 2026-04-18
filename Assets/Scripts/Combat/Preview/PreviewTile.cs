using RPG.Combat.Grid;
using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Preview
{
    [RequireComponent(typeof(CursorTarget))]
    public class PreviewTile : MonoBehaviour
    {
        private PreviewTileInfo _info;
        private CursorTarget _cursorTarget;
        [SerializeField] private List<Collider> _colliders;

        #region Properties

        public PreviewTileInfo Info { get { return _info; } }

        #endregion

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
        }

        private void Start()
        {
            _cursorTarget.Actions.LeftClick.OnCancel(Select);
        }

        public void SetPosition(Vector2Int tilePosition)
        {
            tilePosition = tilePosition.ClampMap();

            for (int i = 0; i < _colliders.Count; i++)
            {
                _colliders[i].gameObject.SetActive(i == tilePosition.y);
                _cursorTarget.SetCollider(_colliders[i]);
            }

            transform.position = MapManager.Instance.GetWorldPostion(tilePosition);
            transform.LookAt(transform.position - (transform.position.normalized));
        }

        public void SetInfo(PreviewTileInfo info)
        {
            _info = info;
        }

        private void Select()
        {
            ActionsManager.Instance.OnActionTileSelected?.Invoke(_info);
        }
    }
}
