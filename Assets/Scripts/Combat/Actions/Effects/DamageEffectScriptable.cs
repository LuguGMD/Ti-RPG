using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [CreateAssetMenu(fileName = "DamageEffectScriptable", menuName = "Scriptable Objects/Effects/Damage")]
    public class DamageEffectScriptable : EffectCommandScriptable
    {
        [SerializeField] private float _damage;

        #region Properties

        public float Damage { get { return _damage; } }

        #endregion

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            Debug.Log("Ataque tentado");

            if (target == null || user == null)
                return false;

            float damage = _damage;
            if (CombatManager.IsTargetWeak(user.Info.Type, target.Info.Type)) damage *= 2;

            target.TakeDamage(damage);

            Debug.Log("Ataque bem sucedido");

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
