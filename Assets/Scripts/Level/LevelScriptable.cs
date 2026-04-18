using RPG.Combat.Wave;
using UnityEngine;

namespace RPG.Level
{
    [CreateAssetMenu(fileName = "LevelScriptable", menuName = "Scriptable Objects/Level/New Level")]
    public class LevelScriptable : ScriptableObject
    {
        [SerializeField] private WaveInfo[] _waves;
        //TO DO adicionar desafios e outras informacoes necessaria para compor uma fase

        #region Properties

        public WaveInfo[] Waves { get { return _waves; } }

        #endregion
    }
}
