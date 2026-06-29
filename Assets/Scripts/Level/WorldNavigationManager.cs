using RPG.Camera;
using RPG.Input;
using RPG.Save;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Level
{
    public class WorldNavigationManager : MonoBehaviour, ISavable<WorldNavigationManager, LevelManagerData, LevelManagerAdapter>
    {
        private PlayerInput _playerInput;

        [Header("Levels")]
        [SerializeField]
        private List<LevelNode> levels = new();

        [Header("Progression")]
        [SerializeField]
        private int maxUnlockedLevel = 0;

        [Header("Current Level")]
        [SerializeField]
        private int currentLevel = 0;
        private string _key = "WorldNavigationManager";

        #region Properties

        public string Key
        {
            set { _key = value; }
            get { return _key; }
        }

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

        private void Awake()
        {
            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        }

        private void Start()
        {
            Load();

            _playerInput.Actions.Move.OnUpdate(NavigationInput);
            _playerInput.Actions.Interact.OnStart(SelectCurrentLevel);

            CameraManager.Instance.SwitchCameraNoBlend(CurrentLevel.LevelCamera);
        }

        private void OnDestroy()
        {
            SaveManager.Save();
        }

        private void NavigationInput(Vector2 input)
        {
            if (CameraManager.Instance.CurrentCamera.IsParticipatingInBlend()) return;
            if (LevelSelectUIController.IsActive) return;

            if(input.y > 0.3f)
            {
                NextLevel();
            }
            else if(input.y < -0.3f)
            {
                PreviousLevel();
            }
        }

        public void NextLevelButton()
        {
            NavigationInput(Vector2.up);
        }

        public void PreviousLevelButton()
        {
            NavigationInput(Vector2.down);
        }

        private void NextLevel()
        {
            int nextIndex = currentLevel + 1; 

            if (nextIndex >= levels.Count) 
                return;

            if (nextIndex > maxUnlockedLevel) 
                return;

            ChangeCurrentLevel(nextIndex);
        }

        private void PreviousLevel()
        {
            int previousIndex = currentLevel - 1; 
            if (previousIndex < 0) 
                return;

            ChangeCurrentLevel(previousIndex);
        }

        public void ChangeCurrentLevel(int levelIndex)
        {
            currentLevel = levelIndex;

            LevelNode _currentLevel = CurrentLevel;

            CameraManager.Instance.SwitchCamera(CurrentLevel.LevelCamera);

            Save();

            UpdateLoadedLevels();

            if (currentLevel == maxUnlockedLevel && currentLevel < levels.Count-1 && GameManager.CompletedLevels.Contains(CurrentLevel.LevelData.LevelKey))
            {
                UnlockNextLevel();
            }
        }

        public void SetMaxUnlockedLevel(int levelIndex)
        {
            maxUnlockedLevel = levelIndex;
            if (currentLevel == maxUnlockedLevel && currentLevel < levels.Count - 1 && GameManager.CompletedLevels.Contains(CurrentLevel.LevelData.LevelKey))
            {
                UnlockNextLevel();
            }
        }

        public void UnlockAllLevels()
        {
            maxUnlockedLevel = levels.Count-1;
        }


        private void SelectCurrentLevel()
        {
            if (CurrentLevel == null) return;
            if (CurrentLevel.LevelData == null) return;
            if (CameraManager.Instance.CurrentCamera.IsParticipatingInBlend()) return;
            if (LevelSelectUIController.IsActive) return;

            ActionsManager.Instance.OnLevelSelected?.Invoke(CurrentLevel.LevelData);
        }

        private void UpdateLoadedLevels()
        {
            for (int i = 0; i < levels.Count;i++)
            {
                bool shouldBeActive = 
                    i == currentLevel ||
                    i == currentLevel - 1 ||
                    i == currentLevel + 1;

                levels[i].SetVisualActive(shouldBeActive);
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
            ISavable<WorldNavigationManager, LevelManagerData, LevelManagerAdapter> savable = (ISavable<WorldNavigationManager, LevelManagerData, LevelManagerAdapter>)this;
            savable.SaveInfo();
        }

        public void Load()
        {
            ISavable<WorldNavigationManager, LevelManagerData, LevelManagerAdapter> savable = (ISavable<WorldNavigationManager, LevelManagerData, LevelManagerAdapter>)this;
            savable.LoadInfo();
        }

        #endregion
    }
}
