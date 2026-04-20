using UnityEngine;

namespace RPG.Combat
{
    
    public abstract class EntityScriptable : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _entityName = "Entity";

        #region Properties

        public string EntityName => _entityName;
        public abstract TeamEnum Team { get; }

        #endregion
    }
}
