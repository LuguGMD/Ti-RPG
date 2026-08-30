using Lugu.Singleton;
using RPG.Combat;
using RPG.Level;
using RPG.Management.Progression;
using RPG.Save;
using RPG.Tutorial;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    public class GameManager : SingletonMonoPersistent<GameManager>, ISavable<GameManager, GameManagerAdapter>
    {
        [SerializeField] private LevelScriptable _selectedLevel;
        [SerializeField] private CharacterScriptable[] _currentParty = new CharacterScriptable[CombatConstants.MAX_CHARACTERS_COUNT];
        [SerializeField] private CharacterScriptable[] _availableCharacters;
        private List<string> _completedChallenges = new List<string>();
        private List<string> _completedLevels = new List<string>();

        private int _coins = 0;
        private string _key = "GameManager";

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }
        public static CharacterScriptable[] CurrentParty { get { return Instance._currentParty; } }
        public static CharacterScriptable[] AvailableCharacters {  get { return Instance._availableCharacters; } }
        public static int Coins { get { return Instance._coins; } }
        public string Key { get { return _key; } set { _key = value; } }
        public static List<string> CompletedChallenges { get  { return Instance._completedChallenges; } }
        public static List<string> CompletedLevels { get  { return Instance._completedLevels; } }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            SaveManager.Instance.LoadAll();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnLevelSelected += SelectLevel;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnLevelSelected -= SelectLevel;
        }

        private void OnDestroy()
        {
            if(Instance == this)
                SaveManager.Instance.SaveAll();
        }

        private void SelectLevel(LevelScriptable selectedLevel)
        {
            _selectedLevel = selectedLevel;
        }

        #region Progression

        [ContextMenu("Add Coins")]
        private void AddTestCoins()
        {
            AddCoins(100);
        }

        public void SetCoins(int coinsAmount)
        {
            _coins = coinsAmount;
            ActionsManager.Instance.OnCoinsAmountChanged?.Invoke();
        }

        public void AddCoins(int coinsAmount)
        {
            SetCoins(_coins + coinsAmount);
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

        public void Save()
        {
            ISavable<GameManager, GameManagerAdapter> savable = (ISavable<GameManager, GameManagerAdapter>)this;
            savable.SaveInfo();
        }

        public void Load()
        {
            ISavable<GameManager, GameManagerAdapter> savable = (ISavable<GameManager, GameManagerAdapter>)this;
            savable.LoadInfo();
        }

        #endregion
    }
}
