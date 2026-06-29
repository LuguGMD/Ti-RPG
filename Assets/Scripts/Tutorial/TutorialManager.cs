using Lugu.Singleton;
using RPG.Save;
using RPG.Tutorial.Handlers;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Tutorial
{
    public class TutorialManager : SingletonMonoPersistent<TutorialManager>, ISavable<TutorialManager, TutorialManagerData, TutorialManagerAdapter>
    {
        private string _key = "Tutorial";
        private Dictionary<string, bool> _tutorialData = new Dictionary<string, bool>();
        TutorialHandler[] _handlers;

        #region Properties
        public string Key { get => _key; set { _key = value; } }
        public Dictionary<string, bool> TutorialData { get { return _tutorialData; } }
        #endregion

        private void Start()
        {
            /*foreach (TutorialHandler handler in _handlers)
            {
                bool isComplete = IsTutorialComplete(handler.TutorialKey);
                handler.enabled = !isComplete;
            }*/
        }

        public void ResetTutorials()
        {
            _tutorialData.Clear();
            Save();
            SaveManager.Save();
        }

        public bool IsTutorialComplete(string tutorialKey)
        {
            if (_tutorialData.ContainsKey(tutorialKey))
            {
                return _tutorialData[tutorialKey];
            }

            return false;
        }

        public void RegisterTutorial(TutorialData tutorialData)
        {
            this._tutorialData[tutorialData.TutorialKey] = tutorialData.IsCompleted;
        }

        public void SetTutorialCompletion(string tutorialKey, bool isCompleted = true)
        {
            _tutorialData[tutorialKey] = isCompleted;
        }

        public void Save()
        {
            ISavable<TutorialManager, TutorialManagerData, TutorialManagerAdapter> savable = (ISavable<TutorialManager, TutorialManagerData, TutorialManagerAdapter>)this;
            savable.SaveInfo();
        }

        public void Load()
        {
            ISavable<TutorialManager, TutorialManagerData, TutorialManagerAdapter> savable = (ISavable<TutorialManager, TutorialManagerData, TutorialManagerAdapter>)this;
            savable.LoadInfo();
        }

    }
}
