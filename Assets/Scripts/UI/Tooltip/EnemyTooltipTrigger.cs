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
        private Tooltip3DSystem _tooltip3DSystem;

        private Tween _delay;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
            _enemyController = GetComponent<EnemyController>();

            _tooltip3DSystem = FindFirstObjectByType<Tooltip3DSystem>();
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
            Debug.Log("1 - HOVER DO INIMIGO DETECTADO!");

            _delay?.Kill();

            _delay = DOVirtual.DelayedCall(0.3f, () =>
            {
                Debug.Log("2 - DELAY DO TOOLTIP EXECUTADO!");

                EnemyScriptable enemy =
                    _enemyController.GetEntityInfo() as EnemyScriptable;

                if (enemy == null)
                {
                    Debug.LogError("3 - INIMIGO É NULL!");
                    return;
                }

                Debug.Log("3 - ENEMY ENCONTRADO: " + enemy.EntityName);

                _tooltip3DSystem.ShowEnemy(
                    enemy,
                    transform.position + new Vector3(_offset.x, _offset.y, 0),
                    _pivot
                );
            });
        }

        private void HideTooltip()
        {
            Debug.Log("HOVER SAIU DO INIMIGO!");

            _delay?.Kill();

            _tooltip3DSystem.Hide();
        }
    }
}