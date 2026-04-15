using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class CombatAction
    {
        [SerializeField] private Effect _effect;
        [SerializeField] private List<Movement> _movementPattern;
        [Tooltip("How many times the movementPattern is repeated")]
        [SerializeField] private int _movementCount;

        #region Properties

        public Effect Effect
        {
            get {  return _effect; }
        }

        public List<Movement> MovementPattern
        {
            get { return _movementPattern; }
        }

        public int MovementCount
        {
            get { return _movementCount; }
        }

        #endregion
    }
}
