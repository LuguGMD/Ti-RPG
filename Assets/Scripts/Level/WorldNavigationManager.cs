using RPG.Camera;
using RPG.Input;
using RPG.Save;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Level
{
    public class WorldNavigationManager : MonoBehaviour, ISavable<WorldNavigationManager, LevelManagerAdapter>
    {

        [Header("Levels")]
        [SerializeField]
        private List<LevelNode> levels = new();

        [Header("Progression")]
        [SerializeField]
        private int maxUnlockedLevel = 0;

        [Header("Current Level")]
        [SerializeField]
        private int currentLevel = 0;

        #region Properties

        private LevelNode CurrentLevel
        {
            get { return levels[currentLevel]; }
        }
        
        public int MaxUnlockedLevel
        {
            get { return maxUnlockedLevel; }
        }

        public int CurrentLevelIndex
        {
            get { return currentLevel; }
        }

        #endregion

        #region Methods

        private void Start()
        {
            Load();
        }

        private void OnDestroy()
        {
            SaveManager.Save();
        }

        public void ChangeCurrentLevel(int levelIndex)
        {
            currentLevel = levelIndex;

            LevelNode _currentLevel = CurrentLevel;

            Save();

            if (currentLevel == maxUnlockedLevel && currentLevel < levels.Count-1 && GameManager.CompletedLevels.Contains(CurrentLevel.LevelData.LevelKey))
            {
                UnlockNextLevel();
            }

            UpdateUnlockedLevels();
        }

        public void SetMaxUnlockedLevel(int levelIndex)
        {
            maxUnlockedLevel = levelIndex;
            if (currentLevel == maxUnlockedLevel && currentLevel < levels.Count - 1 && GameManager.CompletedLevels.Contains(CurrentLevel.LevelData.LevelKey))
            {
                UnlockNextLevel();
            }
            UpdateUnlockedLevels();
        }

        public void UnlockAllLevels()
        {
            maxUnlockedLevel = levels.Count-1;
            UpdateUnlockedLevels();
        }

        private void UpdateUnlockedLevels()
        {
            foreach (LevelNode level in levels)
            {
                if (levels.IndexOf(level) <= maxUnlockedLevel)
                {
                    level.SetEnabled();
                }
                else
                {
                    level.SetDisabled();
                }
            }
        }

        public void UnlockNextLevel()
        {
            if (maxUnlockedLevel < levels.Count - 1)
            {
                maxUnlockedLevel = currentLevel+1;
            }
        }

        public void Save()
        {
            ISavable<WorldNavigationManager, LevelManagerAdapter> savable = (ISavable<WorldNavigationManager, LevelManagerAdapter>)this;
            savable.SaveInfo();
        }

        public void Load()
        {
            ISavable<WorldNavigationManager, LevelManagerAdapter> savable = (ISavable<WorldNavigationManager, LevelManagerAdapter>)this;
            savable.LoadInfo();
        }

        #endregion
    }
}
