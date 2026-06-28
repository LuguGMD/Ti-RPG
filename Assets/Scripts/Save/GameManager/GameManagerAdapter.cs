using UnityEngine;

namespace RPG.Save
{
    public class GameManagerAdapter : SaveAdapter<GameManager, GameManagerData>
    {
        public override void ClassToData(GameManager classSave, GameManagerData dataSave)
        {
            dataSave.Coins = GameManager.Coins;
            dataSave.DefeatedCharacters = GameManager.DefeatedCharacters.ToArray();
        }

        public override void DataToClass(GameManager classSave, GameManagerData dataSave)
        {
            classSave.AddCoins(dataSave.Coins);
            if (dataSave.DefeatedCharacters != null)
            {
                GameManager.Instance.LoadCharacterDemotivated(dataSave.DefeatedCharacters);
            }
        }
    }
}
