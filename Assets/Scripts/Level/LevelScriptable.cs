using RPG.Combat.Wave;
using RPG.Combat.Challenge;
using System;
using UnityEngine;

namespace RPG.Level
{
    [CreateAssetMenu(fileName = "LevelScriptable", menuName = "Scriptable Objects/Level/New Level")]
    public class LevelScriptable : ScriptableObject
    {
        [SerializeField] private WaveInfo[] _waves;
        [SerializeField] private ChallengeScriptable[] _challenges;
        [SerializeField] public string levelName;
        [SerializeField] private int _coinsReward = 50;
        //TO DO adicionar desafios e outras informacoes necessaria para compor uma fase

        #region Properties

        public WaveInfo[] Waves { get { return _waves; } }
        public ChallengeScriptable[] Challenges { get { return _challenges; } }
        public int CoinsReward { get { return _coinsReward; } }

        #endregion
    }
}
