using RPG.Combat;
using UnityEngine;

namespace RPG.Save
{
    [System.Serializable]
    public class GameManagerData
    {
        public int Coins;
        public string[] CompletedChallenges;
        public string[] CompletedLevels;
    }
}
