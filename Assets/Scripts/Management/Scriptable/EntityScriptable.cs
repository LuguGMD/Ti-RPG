using UnityEngine;

namespace RPG.Management
{
    public abstract class EntityScriptable : ScriptableObject
    {
        [SerializeField] private string _characterName;
        [SerializeField] private Sprite _icon;

        #region Properties

        public string CharacterName { get { return _characterName; } }
        public Sprite Icon { get { return _icon; } }

        #endregion
    }
}
