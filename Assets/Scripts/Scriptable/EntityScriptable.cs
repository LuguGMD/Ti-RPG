using RPG.Combat;
using RPG.Combat.Actions;
using UnityEngine;

namespace RPG
{
    
    public class EntityScriptable : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _entityName = "Entity";
        [SerializeField] private CombatType _type;
        [SerializeField] private CombatAction[] _actions;

        #region Properties

        public string EntityName => _entityName;
        public CombatType Type => _type;

        #endregion
    }
}
