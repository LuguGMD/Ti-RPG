using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public abstract class BossCombatAction : CombatAction
    {
        [SerializeField] private int _actionCost;
        [SerializeField] private int _actionPriority;


        #region Properties

        public int ActionCost { get { return _actionCost; } }
        public int ActionPriority { get { return _actionPriority; } }
        
        #endregion


        public virtual bool CheckRequirements()
        {
            // TODO: Ao implementar em classes filhas, considerar usar:
            // CombatManager.RemainingEnemies
            // CombatManager.CurrentTurn
            return true;
        }
    }
}
