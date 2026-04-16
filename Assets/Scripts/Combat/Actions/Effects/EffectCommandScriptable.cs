using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    public abstract class EffectCommandScriptable : ScriptableObject
    {
        public abstract bool Execute(StageEntityController user, StageEntityController target);
        public abstract bool ExecuteApresentador(StageEntityController user, ApresentadorController target);
    }
}
