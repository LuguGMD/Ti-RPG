using Lugu.Singleton;
using RPG.Combat;
using RPG.Level;
using RPG.Management.Progression;
using RPG.Save;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    public class GameManager : SingletonMonoPersistent<GameManager>, ISavable<GameManager, GameManagerData, GameManagerAdapter>
    {
        [SerializeField] private LevelScriptable _selectedLevel;
        private CharacterScriptable _selectedCharacterMinigame;
        [SerializeField] private CharacterScriptable[] _currentParty = new CharacterScriptable[CombatConstants.MAX_CHARACTERS_COUNT];
        [SerializeField] private CharacterScriptable[] _availableCharacters;
        private List<CharacterScriptable> _defeatedCharacters = new List<CharacterScriptable>();
        private List<string> _completedChallenges = new List<string>();
        private List<string> _completedLevels = new List<string>();
        private List<string> _completedTutorials = new List<string>();

        private int _coins = 0;
        private string _key = "GameManager";

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }
        public static CharacterScriptable SelectedCharacterMinigame { get { return Instance._selectedCharacterMinigame; } }
        public static CharacterScriptable[] CurrentParty { get { return Instance._currentParty; } }
        public static CharacterScriptable[] AvailableCharacters {  get { return Instance._availableCharacters; } }
        public static List<CharacterScriptable> DefeatedCharacters { get { return Instance._defeatedCharacters; } }
        public static int Coins { get { return Instance._coins; } }
        public string Key { get { return _key; } set { _key = value; } }
        public static List<string> CompletedChallenges { get  { return Instance._completedChallenges; } }
        public static List<string> CompletedLevels { get  { return Instance._completedLevels; } }
        public static List<string> CompletedTutorials { get { return Instance._completedTutorials; } }

        #endregion

        private void Start()
        {
            SaveManager.Instance.LoadAll();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnLevelSelected += SelectLevel;
            ActionsManager.Instance.OnCharacterMinigameSelected += SelectCharacterMinigame;
            ActionsManager.Instance.OnCharacterDefeated += CharacterDemotivated;
            ActionsManager.Instance.OnCharacterMotivated += CharacterMotivated;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnLevelSelected -= SelectLevel;
            ActionsManager.Instance.OnCharacterMinigameSelected -= SelectCharacterMinigame;
            ActionsManager.Instance.OnCharacterDefeated -= CharacterDemotivated;
            ActionsManager.Instance.OnCharacterMotivated -= CharacterMotivated;
        }

        private void OnDestroy()
        {
            SaveManager.Instance.SaveAll();
        }

        private void SelectLevel(LevelScriptable selectedLevel)
        {
            _selectedLevel = selectedLevel;
        }

        private void SelectCharacterMinigame(CharacterScriptable character)
        {
            _selectedCharacterMinigame = character;
            ChangeScene(ScenesEnum.MinigameDefault);
        }

        private void CharacterDemotivated(CharacterController character)
        {
            if(!_defeatedCharacters.Contains(character.CharacterInfo))
            {
                _defeatedCharacters.Add(character.CharacterInfo);
            }
        }

        public void LoadCharacterDemotivated(string[] characters)
        {
            _defeatedCharacters = new List<CharacterScriptable>();

            foreach (string name in characters)
            {
                foreach(CharacterScriptable character in _availableCharacters)
                {
                    if(character.EntityName == name)
                        _defeatedCharacters.Add(character);
                }
            }
        }

        private void CharacterMotivated(CharacterScriptable character)
        {
            if (_defeatedCharacters.Contains(character))
            {
                _defeatedCharacters.Remove(character);
            }

            //TO DO mover para outro lugar
            ChangeScene(ScenesEnum.Management);
        }

        #region Progression

        [ContextMenu("Add Coins")]
        private void AddTestCoins()
        {
            AddCoins(100);
        }

        public void AddCoins(int coinsAmount)
        {
            _coins += coinsAmount;
            ActionsManager.Instance.OnCoinsAmountChanged?.Invoke();
        }

        public void SpendCoins(int coinsAmount)
        {
            _coins -= coinsAmount;
            ActionsManager.Instance.OnCoinsAmountChanged?.Invoke();
        }

        public void CompleteChallenge(string challengeKey)
        {
            _completedChallenges.Add(challengeKey);
        }

        public void CompleteLevel(string levelKey)
        {
            _completedLevels.Add(levelKey);
        }


        #endregion

        #region General

        public static void ChangeScene(ScenesEnum scene)
        {
            ChangeScene((int)scene);
        }

        public static void ChangeScene(string name)
        {
            ChangeScene(SceneManager.GetSceneByName(name).buildIndex);
        }

        public static void ChangeScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public static void LoadAdditiveScene(ScenesEnum scene)
        {
            SceneManager.LoadScene((int)scene, LoadSceneMode.Additive);
        }

        public static void UnloadAdditiveScene(ScenesEnum scene)
        {
            SceneManager.UnloadSceneAsync((int)scene);
        }

        public void CompleteTutorial(string tutorialKey)
        {
            _completedTutorials.Add(tutorialKey);
        }

        public static void ResetTutorials()
        {
            Instance._completedTutorials = new List<string>();
        }

        public void Save()
        {
            ISavable<GameManager, GameManagerData, GameManagerAdapter> savable = (ISavable<GameManager, GameManagerData, GameManagerAdapter>)this;
            savable.SaveInfo();
        }

        public void Load()
        {
            ISavable<GameManager, GameManagerData, GameManagerAdapter> savable = (ISavable<GameManager, GameManagerData, GameManagerAdapter>)this;
            savable.LoadInfo();
        }

        #endregion
    }
}
