using RPG.Combat.Actions;
using RPG.Combat.Actions.Effects;
using UnityEngine;

namespace RPG.Combat
{
    public class BossSpawnedEnemyController : EnemyController
    {
        #region Properties

        [SerializeField] private float _damageBuffMultiplier = 1.5f;
        private bool _isBuffActive = false;
        private bool IsOutOfSpotlight { get { return !_tileObject.IsOnSpotlight; } }

        #endregion

        #region Methods

        protected new void OnEnable()
        {
            base.OnEnable();
            SubscribeToSpotlightChanges();
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            UnsubscribeFromSpotlightChanges();
        }

        private void SubscribeToSpotlightChanges()
        {
            if (_tileObject != null)
            {
                _tileObject.OnSpotlightStateChange += OnSpotlightStateChanged;
            }
        }

        private void UnsubscribeFromSpotlightChanges()
        {
            if (_tileObject != null)
            {
                _tileObject.OnSpotlightStateChange -= OnSpotlightStateChanged;
            }
        }

        private void OnSpotlightStateChanged(bool isOnSpotlight)
        {
            if (IsOutOfSpotlight && !_isBuffActive)
            {
                ApplyDamageBuff();
            }
            else if (!IsOutOfSpotlight && _isBuffActive)
            {
                RemoveDamageBuff();
            }
        }

        private void ApplyDamageBuff()
        {
            _isBuffActive = true;

            foreach (CombatAction action in _actions)
            {
                ApplyBuffToAction(action);
            }
        }

        private void RemoveDamageBuff()
        {
            _isBuffActive = false;

            foreach (CombatAction action in _actions)
            {
                RemoveBuffFromAction(action);
            }
        }

        private void ApplyBuffToAction(CombatAction action)
        {
            if (action == null) return;

            foreach (Effect effect in action.Effects)
            {
                if (effect == null) continue;

                foreach (EffectCommand command in effect.Commands)
                {
                    if (command is DamageEffect damageEffect)
                    {
                        damageEffect.ApplyDamageMultiplier(_damageBuffMultiplier);
                    }
                }
            }
        }

        private void RemoveBuffFromAction(CombatAction action)
        {
            if (action == null) return;

            foreach (Effect effect in action.Effects)
            {
                if (effect == null) continue;

                foreach (EffectCommand command in effect.Commands)
                {
                    if (command is DamageEffect damageEffect)
                    {
                        damageEffect.RemoveDamageMultiplier(_damageBuffMultiplier);
                    }
                }
            }
        }

        #endregion
    }
}
