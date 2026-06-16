using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueWithChoice : Dialogue
    {
        [SerializeField] private Dialogue[] choices;
        public IReadOnlyList<Dialogue> Choices => choices;
        public void SetChoices(params Dialogue[] choices)
        {
            this.choices = choices;
        }

        public void Select(int index)
        {
            Dialogue entry = choices[index];
            Next = entry;
        }
    }
}
