using UnityEngine;

namespace RPG.Save
{
    public class GameManagerAdapter : SaveAdapter<GameManager, GameManagerData>
    {
        public override void ClassToData(GameManager classSave, GameManagerData dataSave)
        {
            dataSave.Coins = GameManager.Coins;
        }

        public override void DataToClass(GameManager classSave, GameManagerData dataSave)
        {
            classSave.AddCoins(dataSave.Coins);
        }
    }
}
