using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [System.Serializable]
    public class DamageEffect : EffectCommand
    {
        private float _damage;

        #region Properties

        public float Damage { get { return _damage; } }

        #endregion

        public DamageEffect(float damage)
        {
            _damage = damage;
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
    }
}
