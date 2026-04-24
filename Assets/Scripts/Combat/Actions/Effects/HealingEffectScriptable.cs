using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [CreateAssetMenu(fileName = "HealingEffectScriptable", menuName = "Scriptable Objects/Combat/Effects/Healing")]
    public class HealingEffectScriptable : EffectCommandScriptable
    {
        [SerializeField] private float _healAmount = 10;

        #region Properties

        public float HealAmount { get { return _healAmount; } }

        #endregion

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            if (target == null) return false;

            target.Heal(_healAmount);

            return true;
        }

        public override bool ExecuteApresentador(StageEntityController user, ApresentadorController target)
        {
            return false;
        }
    }
}
