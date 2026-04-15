using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    public abstract class EffectCommandScriptable : ScriptableObject
    {
        public abstract bool Execute(EntityController user, EntityController target);
    }
}
