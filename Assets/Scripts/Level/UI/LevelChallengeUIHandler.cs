using RPG.Combat.Challenge;
using TMPro;
using UnityEngine;

namespace RPG.Level
{
    public class LevelChallengeUIHandler : MonoBehaviour
    {
        private ChallengeScriptable _info;
        [SerializeField] private TextMeshProUGUI _challengeNameText;
        [SerializeField] private TextMeshProUGUI _challengeRewardText;
        public void UpdateInfo(ChallengeScriptable info)
        {
            _info = info;

            _challengeNameText.text = _info.ChallengeName;
            _challengeRewardText.text = _info.CoinsReward.ToString() + " Moedas";
        }
    }
}
