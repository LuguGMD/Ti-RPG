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

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }
        public static CharacterScriptable[] CurrentParty { get { return Instance._currentParty; } }

        #endregion

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
