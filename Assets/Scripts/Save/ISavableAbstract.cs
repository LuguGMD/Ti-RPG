using UnityEngine;

namespace RPG.Save
{
    public interface ISavableAbstract
    {
        public abstract void Save();
        public abstract void Load();
    }
}
