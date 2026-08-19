using Lugu.Singleton;
using System;
using System.IO;
using System.Linq;
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
        private SaveData _saveData = new SaveData();

        public static SaveData SaveData
        { 
            get 
            {
                return Instance._saveData; 
            }
        }

        public void FindSavables()
        {
            _onSave = null;
            _onLoad = null;

            var isavables = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavableAbstract>();

            foreach (ISavableAbstract isavable in isavables)
            {
                _onSave += isavable.Save;
                _onLoad += isavable.Load;
            }
        }

        public void SaveAll()
        {
            FindSavables();

            _onSave?.Invoke();
            _onSaveEnded?.Invoke();

            File.WriteAllText(GetSaveFilePath(), JsonUtility.ToJson(_saveData, true));
        }

        public void LoadAll()
        {
            FindSavables();

            string path = GetSaveFilePath();
            string json = null;

            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                _saveData = JsonUtility.FromJson<SaveData>(json);
            }

            _onLoad?.Invoke();
            _onLoadEnded?.Invoke();
        }

        public static void Save()
        {
            File.WriteAllText(GetSaveFilePath(), JsonUtility.ToJson(Instance._saveData, true));
        }

        public static void ResetSave()
        {
            if (File.Exists(GetSaveFilePath()))
            {
                File.Delete(GetSaveFilePath());
            }

            Instance._saveData = new SaveData();
        }

        private static string GetSaveFilePath()
        {
            string path = Path.Combine(Application.persistentDataPath, SAVE_FILE);
            path += ".json";
            return path;
        }
    }
}
