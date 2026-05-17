using Lugu.Singleton;
using RPG.Combat;
using RPG.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class GameManager : SingletonMonoPersistent<GameManager>
    {
        [SerializeField] private LevelScriptable _selectedLevel;
        [SerializeField] private CharacterScriptable[] _currentParty = new CharacterScriptable[CombatConstants.MAX_CHARACTERS_COUNT];
        [SerializeField] private CharacterScriptable[] _availableCharacters;

        private int _coins = 0;

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }
        public static CharacterScriptable[] CurrentParty { get { return Instance._currentParty; } }
        public static CharacterScriptable[] AvailableCharacters {  get { return Instance._availableCharacters; } }
        public static int Coins { get { return Instance._coins; } }

        #endregion

        private void OnEnable()
        {
            ActionsManager.Instance.OnLevelSelected += SelectLevel;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnLevelSelected -= SelectLevel;
        }

        private void SelectLevel(LevelScriptable selectedLevel)
        {
            _selectedLevel = selectedLevel;
        }

        public void AddCoins(int coinsAmount)
        {
            _coins += coinsAmount;
        }

        public void SpendCoins(int coinsAmount)
        {
            _coins -= coinsAmount;
        }

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
    }
}
