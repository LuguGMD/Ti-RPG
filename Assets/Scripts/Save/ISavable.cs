using UnityEngine;

namespace RPG.Save
{
    public interface ISavable<Class, Data, Adapter> : ISavableAbstract where Class : ISavableAbstract where Data : class, new() where Adapter : SaveAdapter<Class, Data>, new()
    {
        public string Key { get; set; }

        public void SaveInfo()
        {
            Adapter adapter = new Adapter();
            Data data = new Data();
            adapter.ClassToData((Class)this, data);
            SaveManager.Save(Key, JsonUtility.ToJson(data, true));
        }

        public void LoadInfo()
        {
            Adapter adapter = new Adapter();
            string json = SaveManager.Load(Key);

            if (json != null)
            {
                Data data = JsonUtility.FromJson<Data>(json);
                adapter.DataToClass((Class)this, data);
            }
        }
    }
}
