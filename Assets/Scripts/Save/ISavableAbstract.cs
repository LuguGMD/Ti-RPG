using UnityEngine;

namespace RPG.Save
{
    public interface ISavableAbstract
    {
        public void Save();
        public void Load();
    }
}
