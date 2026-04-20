using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class CombatAction
    {
        [SerializeField] private string _actionName;
        [SerializeField] private List<Effect> _effects;
        [SerializeField] private List<MovementPattern> _movementPatterns;

        #region Properties

        public List<Effect> Effects
        {
            get {  return _effects; }
        }

        public List<MovementPattern> MovementPatterns
        {
            get { return _movementPatterns; }
        }

        #endregion

        public List<EffectTriggerEnum> GetEffectTriggers()
        {
            List<EffectTriggerEnum> effectTriggers = new List<EffectTriggerEnum>();
            for (int i = 0; i < _effects.Count; i++)
            {
                effectTriggers.Add(_effects[i].TriggerCondition);
            }
            return effectTriggers;
        }
    }
}
