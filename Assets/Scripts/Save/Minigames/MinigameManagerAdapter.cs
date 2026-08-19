using RPG.Management.Minigames;
using UnityEngine;

namespace RPG.Save
{
    public class MinigameManagerAdapter : SaveAdapter<MinigameManager>
    {
        public override void ClassToData(MinigameManager classSave)
        {
            MinigameManagerData dataSave = SaveManager.SaveData.MinigameManagerData;
            dataSave.ComboRecord = MinigameManager.ComboRecord;
            dataSave.ComboPerfectRecord = MinigameManager.ComboPerfectRecord;
            dataSave.CompletedChallengesCount = MinigameManager.CurrentChallengeIndex;
        }

        public override void DataToClass(MinigameManager classSave)
        {
            MinigameManagerData dataSave = SaveManager.SaveData.MinigameManagerData;
            classSave.Init(dataSave);
        }
    }
}
