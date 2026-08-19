using UnityEngine;

namespace RPG.Save
{
    public interface ISavable<Class, Adapter> : ISavableAbstract where Class : ISavableAbstract where Adapter : SaveAdapter<Class>, new()
    {
        public string Key { get; set; }

        public void SaveInfo()
        {
            Adapter adapter = new Adapter();
            adapter.ClassToData((Class)this);
        }

        public void LoadInfo()
        {
            Adapter adapter = new Adapter();
            adapter.DataToClass((Class)this);
        }
    }
}
