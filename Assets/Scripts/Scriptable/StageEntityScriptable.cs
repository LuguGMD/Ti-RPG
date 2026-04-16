using RPG.Combat;
using RPG.Combat.Actions;
using UnityEngine;

namespace RPG
{
    public abstract class StageEntityScriptable : EntityScriptable
    {
        [SerializeField] private CombatAction[] _actions;
        [SerializeField] private CombatType _type;

        #region Properties

        public CombatAction[] Actions => _actions;
        public CombatType Type => _type;

        #endregion
    }
}
