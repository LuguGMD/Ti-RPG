using RPG.Management.Progression;

namespace RPG.Save
{
    public class UpgradeGraphAdapter : SaveAdapter<UpgradeGraphUI>
    {
        public override void ClassToData(UpgradeGraphUI classSave)
        {
            UpgradeGraphData dataSave = SaveManager.SaveData.UpgradeGraphData;
            dataSave.PurchasedUpgradeIDs = classSave.GetPurchasedUpgradeIDs();
        }

        public override void DataToClass(UpgradeGraphUI classSave)
        {
            UpgradeGraphData dataSave = SaveManager.SaveData.UpgradeGraphData;
            classSave.ApplyPurchasedUpgradeIDs(dataSave.PurchasedUpgradeIDs);
        }
    }
}
