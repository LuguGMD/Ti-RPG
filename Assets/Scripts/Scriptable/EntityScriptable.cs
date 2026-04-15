using UnityEngine;

namespace RPG
{
    
    public class EntityScriptable : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _entityName = "Entity";

        #region Properties
        public string EntityName => _entityName;

        #endregion
    }
}
