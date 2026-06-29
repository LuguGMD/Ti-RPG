using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
    public static class SaveConstants
    {
        public enum SaveKey
        { 
            GameManager,
            Upgrade,
            Dialogue,
            Level,
            MinigameRhythm,
            Audio,
            Tutorial,
        }

        public static Dictionary<SaveKey, string> SaveKeys = new Dictionary<SaveKey, string>()
        {
            {SaveKey.GameManager, "GameManager"},
            {SaveKey.Upgrade, "UpgradeGraph"},
            {SaveKey.Dialogue, "DialogueManager"},
            {SaveKey.Level, "WorldNavigationManager"},
            {SaveKey.MinigameRhythm, "RhythmMinigame"},
            {SaveKey.Audio, "AudioManager"},
            {SaveKey.Tutorial, "Tutorial"},
        };

    }
}
