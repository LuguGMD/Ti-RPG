using RPG.Management.Minigames;
using UnityEngine;

namespace RPG.Save
{
    public class MinigameManagerAdapter : SaveAdapter<MinigameManager, MinigameManagerData>
    {
        public override void ClassToData(MinigameManager classSave, MinigameManagerData dataSave)
        {
            dataSave.ComboRecord = MinigameManager.ComboRecord;
            dataSave.ComboPerfectRecord = MinigameManager.ComboPerfectRecord;
            dataSave.CompletedChallengesCount = MinigameManager.CurrentChallengeIndex;
        }

        public override void DataToClass(MinigameManager classSave, MinigameManagerData dataSave)
        {
            classSave.Init(dataSave);
        }
    }
}
