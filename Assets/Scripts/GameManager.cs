using Lugu.Singleton;
using RPG.Level;
using UnityEngine;

namespace RPG
{
    public class GameManager : SingletonMonoPersistent<GameManager>
    {
        [SerializeField] private LevelScriptable _selectedLevel;

        #region Properties

        public static LevelScriptable SelectedLevel { get { return Instance._selectedLevel; } }

        #endregion
    }
}
