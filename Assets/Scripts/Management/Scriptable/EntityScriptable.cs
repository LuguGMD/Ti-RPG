using UnityEngine;

namespace RPG.Management
{
    public abstract class EntityScriptable : ScriptableObject
    {
        [SerializeField] private string _characterName;

        #region Properties

        public string CharacterName { get { return _characterName; } }

        #endregion
    }
}
