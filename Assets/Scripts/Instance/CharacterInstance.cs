using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public class CharacterInstance
    {
        [SerializeField] private CharacterScriptable data;
        [SerializeField] private int currentMotivation;

        public CharacterInstance(CharacterScriptable data)
        {
            this.data = data;
            currentMotivation = data != null ? data.Motivation : 0;
        }

        public CharacterScriptable Data => data;
        public string Name => data != null ? data.CharacterName : "(No Data)";
        public int Motivation => data != null ? data.Motivation : 0;

        public int CurrentMotivation => currentMotivation;
        public bool IsAlive => currentMotivation > 0;

    }
}
