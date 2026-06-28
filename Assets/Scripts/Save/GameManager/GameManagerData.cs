using RPG.Combat;
using UnityEngine;

namespace RPG.Save
{
    [System.Serializable]
    public class GameManagerData
    {
        public int Coins;
        public CharacterScriptable[] DefeatedCharacters;
        public string[] CompletedChallenges;
        public string[] CompletedLevels;
        public string[] CompletedTutorials;
    }
}
