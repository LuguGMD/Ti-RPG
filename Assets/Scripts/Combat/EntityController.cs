using RPG.Combat.Actions;
using RPG.Combat.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(TileObjectMovement))]
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] private EntityScriptable _info;
        private TileObjectMovement _movement;

        #region Properties

        public EntityScriptable Info { get { return _info; } }
        public TileObjectMovement Movement { get { return _movement; } }
        public Vector2Int Position { get { return _movement.TileObject.Position; } }
        public Direction Direction { get { return _movement.Direction; } }

        #endregion

        private void Awake()
        {
            _movement = GetComponent<TileObjectMovement>();
        }

        public abstract void TakeDamage(float damage);

        public IEnumerator UseAction(int actionIndex, int movementPatternIndex, int repetitions, bool isMirrored)
        {
            CombatAction action = _info.Actions[actionIndex];
            MovementPattern movementPattern = action.MovementPatterns[movementPatternIndex];

            SubscribeEffects(action.Effects);

            _movement.EnqueuePattern(movementPattern.Pattern, repetitions);

            ActionsManager.Instance.OnActionStart?.Invoke();

            yield return new WaitForSeconds(0.2f);

            while (_movement.MovementQueue.Count > 0)
            {
                yield return new WaitForSeconds(0.1f);
                _movement.Move();
            }

            ActionsManager.Instance.OnActionEnd?.Invoke();

            yield return new WaitForSeconds(0.5f);

            CombatManager.UnsubscribeEffectTriggerAction();
        }

        private void SubscribeEffects(List<Effect> effects)
        {
            foreach(Effect effect in effects)
            {
                CombatManager.SubscribeEffectTriggerAction(effect.TriggerCondition, () => effect.Execute(this));
            }
        }

        [ContextMenu("Use Action")]
        private void UseActionTest()
        {
            StartCoroutine(UseAction(0, 0, 1, false));
        }
    }
}
