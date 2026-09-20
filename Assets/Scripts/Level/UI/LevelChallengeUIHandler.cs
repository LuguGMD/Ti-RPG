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
        [SerializeField] private Image _completedIcon;
        public void UpdateInfo(ChallengeScriptable info)
        {
            _info = info;

            _completedIcon.color = GameManager.CompletedChallenges.Contains(info.ChallengeKey) ? Color.white : Color.gray3;

            _challengeNameText.text = _info.ChallengeName;
        }
    }
}
