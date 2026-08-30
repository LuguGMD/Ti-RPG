using RPG.Level;
using UnityEngine;


namespace RPG.Save
{
    public class LevelManagerAdapter : SaveAdapter<WorldNavigationManager>
    {
        public override void ClassToData(WorldNavigationManager classSave)
        {
            LevelManagerData dataSave = SaveManager.SaveData.LevelManagerData;
            dataSave.maxUnlockedLevel = classSave.MaxUnlockedLevel;
            dataSave.currentLevel = classSave.CurrentLevelIndex;
        }

        public override void DataToClass(WorldNavigationManager classSave)
        {
            LevelManagerData dataSave = SaveManager.SaveData.LevelManagerData;
            classSave.ChangeCurrentLevel(dataSave.currentLevel);
            classSave.SetMaxUnlockedLevel(dataSave.maxUnlockedLevel);
        }
    }
}