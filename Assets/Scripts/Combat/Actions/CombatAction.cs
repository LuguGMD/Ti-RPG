using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class CombatAction
    {
        [SerializeField] private string _actionName;
        [SerializeField] private string _actionDescription;
        [SerializeField] private List<Effect> _effects;
        [SerializeField] private List<MovementPattern> _movementPatterns;
        [SerializeField] private bool _lastTileNeedsToBeEmpty = true;

        #region Properties

        public string ActionName { get { return  _actionName; }  }
        public string ActionDescription { get { return _actionDescription; } }

        public List<Effect> Effects
        {
            get {  return _effects; }
        }

        public List<MovementPattern> MovementPatterns
        {
            get { return _movementPatterns; }
        }

        public bool LastTileNeedsToBeEmpty { get { return _lastTileNeedsToBeEmpty; } }

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
