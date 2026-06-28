using Lugu.Singleton;
using System;
using System.IO;
using UnityEngine;

namespace RPG.Save
{
    public class SaveManager : Singleton<SaveManager>
    {
        private Action _onSave;
        private Action _onLoad;

        public Action _onSaveEnded;
        public Action _onLoadEnded;

        public const string SAVE_FILE = "save";

        public void RegisterSavable(ISavable savable)
        {
            _onSave += savable.Save;
        }

        public void UnregisterSavable(ISavable savable)
        {
            _onSave -= savable.Save;
        }

        public void RegisterLoadable(ILoadable loadable)
        {
            _onLoad += loadable.Load;
        }

        public void UnregisterLoadable(ILoadable loadable)
        {
            _onLoad -= loadable.Load;
        }

        public void SaveAll()
        {
            _onSave?.Invoke();
            _onSaveEnded?.Invoke();
        }

        public void LoadAll()
        {
            _onLoad?.Invoke();
            _onLoadEnded?.Invoke();
        }

        public static void Save(string json, string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = SAVE_FILE;
            }

            path = Path.Combine(Application.persistentDataPath, path);
            path += ".json";

            File.WriteAllText(path, json);

        }

        public static string Load(string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = SAVE_FILE;
            }

            path = Path.Combine(Application.persistentDataPath, path);
            path += ".json";

            string json = null;

            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
            }

            return json;
        }
    }
}
