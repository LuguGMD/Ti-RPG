using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
    [System.Serializable]
    public class SaveData
    {
        public List<JsonEntry> Entries = new List<JsonEntry>();
    }
}
