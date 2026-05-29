using Lugu.Singleton;
using RPG.Combat;
using RPG.Level;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    public class GameManager : SingletonMonoPersistent<GameManager>
    {
        [SerializeField] private LevelScriptable _selectedLevel;
        private CharacterScriptable _selectedCharacterMinigame;
        [SerializeField] private CharacterScriptable[] _currentParty = new CharacterScriptable[CombatConstants.MAX_CHARACTERS_COUNT];
        [SerializeField] private CharacterScriptable[] _availableCharacters;
        private List<CharacterScriptable> _defeatedCharacters = new List<CharacterScriptable>();

        private int _coins = 0;

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }
        public static CharacterScriptable SelectedCharacterMinigame { get { return Instance._selectedCharacterMinigame; } }
        public static CharacterScriptable[] CurrentParty { get { return Instance._currentParty; } }
        public static CharacterScriptable[] AvailableCharacters {  get { return Instance._availableCharacters; } }
        public static List<CharacterScriptable> DefeatedCharacters { get { return Instance._defeatedCharacters; } }
        public static int Coins { get { return Instance._coins; } }

        #endregion

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
                //TO DO Adicionar dnv depois
                //_defeatedCharacters.Add(character.CharacterInfo);
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

        public void AddCoins(int coinsAmount)
        {
            _coins += coinsAmount;
        }

        public void SpendCoins(int coinsAmount)
        {
            _coins -= coinsAmount;
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

        #endregion
    }
}
