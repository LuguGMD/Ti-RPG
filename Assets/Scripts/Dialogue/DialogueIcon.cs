using UnityEngine;

namespace RPG.Management
{
    [System.Serializable]
    public class DialogueIcon
    {
        [SerializeField] private string _key;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;

        #region Properties

        public string Key { get { return _key; } }
        public Sprite Icon { get { return _icon; } }
        public string Name { get { return _name; } }

        #endregion
    }
}
