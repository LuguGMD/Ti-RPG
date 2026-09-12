using DG.Tweening;
using UnityEngine;
using RPG.Combat;
using RPG.Input;

namespace RPG.UI.Tooltip
{
    public class EnemyTooltipTrigger : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _pivot;

        private CursorTarget _cursorTarget;
        private EnemyController _enemyController;

        private Tween _delay;
        private bool _wasHovered;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
            _enemyController = GetComponent<EnemyController>();
        }

        private void Update()
        {
            bool isHovered = _cursorTarget.Actions.Hover.IsPressed;

            if (isHovered && !_wasHovered)
            {
                _wasHovered = true;

                _delay = DOVirtual.DelayedCall(0.3f, () =>
                {
                    if (_cursorTarget.Actions.Hover.IsPressed)
                    {
                        EnemyScriptable enemy =
                            _enemyController.GetEntityInfo() as EnemyScriptable;

                        TooltipSystem.ShowEnemy(
                            enemy,
                            transform.position + new Vector3(_offset.x, _offset.y, 0),
                            _pivot
                        );
                    }
                });
            }

            if (!isHovered && _wasHovered)
            {
                _wasHovered = false;

                _delay?.Kill();
                TooltipSystem.Hide();
            }
        }

        private void OnDisable()
        {
            _delay?.Kill();
        }
    }
}