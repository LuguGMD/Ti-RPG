using DG.Tweening;
using UnityEngine;
using RPG.Input;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class EnemyTooltipTrigger : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _pivot;

        private CursorTarget _cursorTarget;
        private EnemyController _enemyController;

        private Tween _delay;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
            _enemyController = GetComponent<EnemyController>();
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
            Debug.Log("HOVER NO INIMIGO!");

            _delay?.Kill();

            _delay = DOVirtual.DelayedCall(0.3f, () =>
            {
                EnemyScriptable enemy =
                    _enemyController.GetEntityInfo() as EnemyScriptable;

                if (enemy == null)
                {
                    Debug.LogWarning("EnemyScriptable não encontrado!");
                    return;
                }

                Tooltip3DSystem.ShowEnemy(
                    enemy,
                    transform.position + new Vector3(_offset.x, _offset.y, 0),
                    _pivot
                );
            });
        }

        private void HideTooltip()
        {
            _delay?.Kill();

            Tooltip3DSystem.Hide();
        }
    }
}