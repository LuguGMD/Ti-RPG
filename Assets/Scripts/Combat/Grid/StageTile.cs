using RPG.Input;
using UnityEngine;

namespace RPG.Combat.Grid
{
    [RequireComponent(typeof(CursorTarget))]
    public class StageTile : MonoBehaviour
    {
        private CursorTarget _cursorTarget;
        [SerializeField] private Vector2Int _position;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
        }

        private void Start()
        {
            _cursorTarget.Actions.Hover.OnStart(OnHover);
        }

        private void OnHover()
        {
            ActionsManager.Instance.OnTileHovered?.Invoke(_position);
        }
    }
}
