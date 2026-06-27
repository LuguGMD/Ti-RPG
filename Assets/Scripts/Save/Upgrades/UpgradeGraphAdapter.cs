using RPG.Management.Progression;

namespace RPG.Save
{
    public class UpgradeGraphAdapter : SaveAdapter<UpgradeGraphUI, UpgradeGraphData>
    {
        public override void ClassToData(UpgradeGraphUI classSave, UpgradeGraphData dataSave)
        {
            dataSave.PurchasedUpgradeIDs = classSave.GetPurchasedUpgradeIDs();
        }

        public override void DataToClass(UpgradeGraphUI classSave, UpgradeGraphData dataSave)
        {
            classSave.ApplyPurchasedUpgradeIDs(dataSave.PurchasedUpgradeIDs);
        }
    }
}
