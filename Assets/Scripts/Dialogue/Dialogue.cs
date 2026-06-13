using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace RPG.Dialogue
{
    public class Dialogue
    {
        static private StringTable localizationTable;
        static public LocalizedStringTable LocalizationTable
        { set => localizationTable = value.GetTable(); }

        readonly public string Key;
        readonly public DialogueDisplayInfo DisplayInfo;

        public Dialogue(string key, DialogueDisplayInfo displayInfo, Dialogue next)
        {
            Key = key;
            DisplayInfo = displayInfo;

            Next = next;
        }

        public Dialogue Next { get; private set; }

        public string LocalizedText => localizationTable[Key].LocalizedValue;

        public class WithChoice : Dialogue
        {
            private Dialogue[] choices;
            public IReadOnlyList<Dialogue> Choices => choices;

            public WithChoice(string key, DialogueDisplayInfo displayInfo, Dialogue[] choices)
            : base(key, displayInfo, null)
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
}
