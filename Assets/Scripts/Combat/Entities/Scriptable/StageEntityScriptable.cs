using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class StageEntityScriptable : EntityScriptable
    {
        [SerializeField] private CombatAction[] _actions;
        [SerializeField] private CombatTypeEnum _type;

        #region Properties

        public CombatAction[] Actions => _actions;
        public CombatTypeEnum Type => _type;

        #endregion
    }
}
