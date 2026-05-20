using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.Challenge.UI
{
    public class ChallengeProgressPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _challengeNameText;
        [SerializeField] private Slider _challengeProgressSlider;

        public void UpdateInfo(ChallengeHandler challenge)
        {
            _challengeNameText.text = challenge.Info.ChallengeName;
            _challengeProgressSlider.value = challenge.CurrentValue / challenge.Info.TargetValue;
        }
    }
}
