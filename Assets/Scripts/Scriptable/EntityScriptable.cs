using RPG.Combat;
using RPG.Combat.Actions;
using UnityEngine;

namespace RPG
{
    
    public abstract class EntityScriptable : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _entityName = "Entity";

        #region Properties

        public string EntityName => _entityName;
        public abstract Team Team { get; }

        #endregion
    }
}
