using UnityEngine;

namespace RPG.Save
{
    public class GameManagerAdapter : SaveAdapter<GameManager, GameManagerData>
    {
        public override void ClassToData(GameManager classSave, GameManagerData dataSave)
        {
            dataSave.Coins = GameManager.Coins;
            dataSave.DefeatedCharacters = GameManager.DefeatedCharacters.ToArray();
            dataSave.CompletedChallenges = GameManager.CompletedChallenges.ToArray();
            dataSave.CompletedLevels = GameManager.CompletedLevels.ToArray();
        }

        public override void DataToClass(GameManager classSave, GameManagerData dataSave)
        {
            classSave.AddCoins(dataSave.Coins);
            if (dataSave.DefeatedCharacters != null)
            {
                GameManager.Instance.LoadCharacterDemotivated(dataSave.DefeatedCharacters);
            }

            if(dataSave.CompletedChallenges != null)
            {
                foreach(string challengeKey in dataSave.CompletedChallenges) 
                { 
                    GameManager.Instance.CompleteChallenge(challengeKey);
                }
            }

            if (dataSave.CompletedLevels != null)
            {
                foreach (string challengeKey in dataSave.CompletedLevels)
                {
                    GameManager.Instance.CompleteChallenge(challengeKey);
                }
            }
        }
    }
}
