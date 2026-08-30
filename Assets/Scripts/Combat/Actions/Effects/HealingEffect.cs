using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [System.Serializable]
    public class HealingEffect : EffectCommand
    {
        [SerializeField] private float _healAmount = 10;
        private bool _targetsApresentador = false;

        #region Properties

        public float HealAmount { get { return _healAmount; } }

        #endregion

        public HealingEffect(float healAmount, bool targetsApresentador = false)
        {
            _healAmount = healAmount;
            _targetsApresentador = targetsApresentador;
        }

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            if (target == null) return false;

            target.Heal(_healAmount);

            return true;
        }

        public override bool ExecuteApresentador(StageEntityController user, ApresentadorController target)
        {
            if (target == null || !_targetsApresentador) return false;

            target.Heal(_healAmount);

            return true;
        }
    }
}
