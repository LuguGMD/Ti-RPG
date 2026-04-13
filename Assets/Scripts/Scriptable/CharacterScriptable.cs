using UnityEngine;

namespace RPG
{
    public class CharacterScriptable : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string characterName = "Character";

        [Header("Stats")]
        [SerializeField] private int motivation = 100;

        public string CharacterName => characterName;
        public int Motivation => motivation;
    }
}
