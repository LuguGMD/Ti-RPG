using RPG.Combat.Grid;
using RPG.Extensions;
using RPG.Input;
using UnityEngine;

namespace RPG.Combat.Preview
{
    public class PreviewTile : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer[] _renderer;
        protected bool _canBeSelected = true;
        protected Vector2Int _tilePosition;

        #region Properties

        public bool CanBeSelected { get { return _canBeSelected; } }

        #endregion

        protected void Start()
        {
            CursorTarget[] cursorTargets = GetComponentsInChildren<CursorTarget>(true);

            foreach (CursorTarget cursorTarget in cursorTargets)
            {
                cursorTarget.Actions.LeftClick.OnCancel(Select);
            }
        }

        protected void OnEnable()
        {
            ActionsManager.Instance.OnRotationAnimationStarted += RemoveParent;
            ActionsManager.Instance.OnRotationAnimationEnded += AddParent;
        }

        protected void OnDisable()
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

        protected void RemoveParent()
        {
            transform.parent = null;
            transform.GetChild(_tilePosition.y)?.gameObject.SetActive(false);
        }

        protected void AddParent()
        {
            transform.parent = MapManager.Map.GetTile(_tilePosition).Transform;
            transform.GetChild(_tilePosition.y)?.gameObject.SetActive(true);
        }

        public void SetMaterial(Material material)
        {
            _renderer[_tilePosition.y].material = material;
        }

        public void SetCanBeSelected(bool canBeSelected)
        {
            _canBeSelected = canBeSelected;
        }

        protected virtual void Select()
        {
            if (_canBeSelected)
            {
                ActionsManager.Instance.OnPreviewTileSelected?.Invoke(_tilePosition);
            }
        }
    }
}
