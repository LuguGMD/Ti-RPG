using Lugu.Singleton;
using RPG.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class GameManager : SingletonMonoPersistent<GameManager>
    {
        [SerializeField] private LevelScriptable _selectedLevel;

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }

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
