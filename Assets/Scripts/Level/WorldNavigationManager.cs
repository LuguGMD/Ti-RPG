using RPG.Camera;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Level
{
    public class WorldNavigationManager : MonoBehaviour
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

        #endregion

        #region Methods

        public void NextLevel()
        {
            int nextIndex = currentLevel + 1; 

            if (nextIndex >= levels.Count) 
                return;

            if (nextIndex > maxUnlockedLevel) 
                return;
              
            SelectLevel(nextIndex);
        }

        public void PreviousLevel()
        {
            int previousIndex = currentLevel - 1; 
            if (previousIndex < 0) 
                return;

            SelectLevel(previousIndex);
        }

        private void SelectLevel(int levelIndex)
        {
            currentLevel = levelIndex;

            LevelNode _currentLevel = CurrentLevel;

            CameraManager.Instance.SwitchCameraForLevel(levelIndex);

            UpdateLoadedLevels();

            ActionsManager.Instance.OnLevelSelected?.Invoke(this);
        }

        private void UpdateLoadedLevels()
        {
            for (int i = 0; i < levels.Count;i++)
            {
                bool shouldBeActive = 
                    i <= currentLevel ||
                    i == currentLevel - 1 ||
                    i == currentLevel + 1;

                levels[i].SetVisualActive(shouldBeActive);
            }
        }

        public void UnlockNextLevel()
        {
            if (maxUnlockedLevel < levels.Count - 1)
            {
                maxUnlockedLevel++;
            }
        }

        #endregion
    }
}
