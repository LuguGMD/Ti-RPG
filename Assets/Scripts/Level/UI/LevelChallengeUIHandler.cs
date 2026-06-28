using RPG.Combat.Challenge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level
{
    public class LevelChallengeUIHandler : MonoBehaviour
    {
        private ChallengeScriptable _info;
        [SerializeField] private TextMeshProUGUI _challengeNameText;
        [SerializeField] private TextMeshProUGUI _challengeRewardText;
        [SerializeField] private Image _completedIcon;
        public void UpdateInfo(ChallengeScriptable info)
        {
            _info = info;

            _completedIcon.enabled = GameManager.CompletedChallenges.Contains(info.ChallengeKey);

            _challengeNameText.text = _info.ChallengeName;
            _challengeRewardText.text = _info.CoinsReward.ToString() + " Moedas";
        }
    }
}
