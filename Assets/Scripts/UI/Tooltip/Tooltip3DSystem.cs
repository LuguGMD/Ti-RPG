using DG.Tweening;
using UnityEngine;
using RPG.Input;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class Tooltip3DSystem : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _pivot;

        private CursorTarget _cursorTarget;
        private Tween _delay;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
        }

        private void OnEnable()
        {
            _cursorTarget.Actions.Hover.OnStart(ShowTooltip);
            _cursorTarget.Actions.Hover.OnCancel(HideTooltip);
        }

        private void OnDisable()
        {
            _cursorTarget.Actions.Hover.Remove.OnStart(ShowTooltip);
            _cursorTarget.Actions.Hover.Remove.OnCancel(HideTooltip);

            _delay?.Kill();
        }

        private void ShowTooltip()
        {
            _delay?.Kill();

            _delay = DOVirtual.DelayedCall(0.3f, () =>
            {
                EnemyController enemyController = GetComponent<EnemyController>();

                if (enemyController == null)
                {
                    Debug.Log("Não encontrou EnemyController!");
                    return;
                }

                EnemyScriptable enemy =
                    enemyController.GetEntityInfo() as EnemyScriptable;

                if (enemy == null)
                {
                    Debug.Log("Não encontrou EnemyScriptable!");
                    return;
                }

                TooltipSystem.ShowEnemy(
                    enemy,
                    transform.position + new Vector3(_offset.x, _offset.y, 0),
                    _pivot
                );
            });
        }

        private void HideTooltip()
        {
            _delay?.Kill();
            TooltipSystem.Hide();
        }
    }
}