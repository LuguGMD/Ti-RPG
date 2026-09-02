using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [System.Serializable]
    public class DamageEffect : EffectCommand
    {
        private float _damage;
        private float _originalDamage;
        private float _damageMultiplier = 1f;

        #region Properties

        public float Damage { get { return _damage; } }

        #endregion

        public DamageEffect(float damage)
        {
            _damage = damage;
            _originalDamage = damage;
        }

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            if (target == null || user == null)
                return false;

            float damage = _damage;

            target.TakeDamage(damage);

            return true;
        }

        public override bool ExecuteApresentador(StageEntityController user, ApresentadorController target)
        {
            if (target == null || user == null)
                return false;

            target.TakeDamage(_damage);

            return true;
        }

        public void ApplyDamageMultiplier(float multiplier)
        {
            _damageMultiplier = multiplier;
            _damage = _originalDamage * _damageMultiplier;
        }

        public void RemoveDamageMultiplier(float multiplier)
        {
            _damageMultiplier = 1f;
            _damage = _originalDamage;
        }
    }
}
