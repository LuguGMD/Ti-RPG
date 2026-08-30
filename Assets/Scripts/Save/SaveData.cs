using System.Collections.Generic;
using UnityEngine;
using RPG.Tutorial;

namespace RPG.Save
{
    [System.Serializable]
    public class SaveData
    {
        public GameManagerData GameManagerData = new GameManagerData();
        public LevelManagerData LevelManagerData = new LevelManagerData();
        public UpgradeGraphData UpgradeGraphData = new UpgradeGraphData();
        public TutorialManagerData TutorialManagerData = new TutorialManagerData();
        public AudioManagerData AudioManagerData = new AudioManagerData();
    }
}
