using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class Dialogue : ScriptableObject
    {
        static private StringTable localizationTable;
        static public LocalizedStringTable LocalizationTable
        { set => localizationTable = value.GetTable(); }

        public string Key;
        public DialogueDisplayInfo DisplayInfo;

        public void Init(string key, DialogueDisplayInfo displayInfo)
        {
            Key = key;
            DisplayInfo = displayInfo;
        }

        public Dialogue Next;

        public string LocalizedText => localizationTable[Key].LocalizedValue;


    }
}
