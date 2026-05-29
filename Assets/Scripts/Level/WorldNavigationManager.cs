using RPG.Camera;
using RPG.Input;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Level
{
    public class WorldNavigationManager : MonoBehaviour
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

        #region Properties

        private LevelNode CurrentLevel
        {
            get { return levels[currentLevel]; }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        }

        private void Start()
        {
            _playerInput.Actions.Move.OnUpdate(NavigationInput);
            _playerInput.Actions.Interact.OnStart(SelectCurrentLevel);
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

        private void ChangeCurrentLevel(int levelIndex)
        {
            currentLevel = levelIndex;

            LevelNode _currentLevel = CurrentLevel;

            CameraManager.Instance.SwitchCamera(CurrentLevel.LevelCamera);

            UpdateLoadedLevels();
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
                maxUnlockedLevel++;
            }
        }

        #endregion
    }
}
