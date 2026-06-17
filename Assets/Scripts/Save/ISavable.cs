using UnityEngine;

namespace RPG.Save
{
    public interface ISavable
    {
        /// <summary>
        /// Save the class using the SaveAdapter<class>.
        /// </summary>
        public void Save();
    }
}
