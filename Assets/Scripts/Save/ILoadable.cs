using UnityEngine;

namespace RPG.Save
{
    public interface ILoadable
    {
        /// <summary>
        /// Load the class using the SaveAdapter<class>.
        /// </summary>
        public void Load();
    }
}
