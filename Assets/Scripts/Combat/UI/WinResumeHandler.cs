using RPG.Combat.Challenge;
using RPG.Level;
using RPG.Save;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class WinResumeHandler : MonoBehaviour
    {
        [SerializeField] private LevelChallengeUIHandler _challengePreviewPrefab;
        private List<LevelChallengeUIHandler> _challenges;
        [SerializeField] private RectTransform _challengesContainer;
        [SerializeField] private TextMeshProUGUI _coinsEarnedText;

        private void Start()
        {
            LevelScriptable currentLevel = GameManager.SelectedLevel;
            if (!GameManager.CompletedLevels.Contains(currentLevel.LevelKey))
            {
                GameManager.Instance.CompleteLevel(currentLevel.LevelKey);
                _coinsEarnedText.text = "Moedas Recebidas: " + currentLevel.CoinsReward;
            }
            else
            {
                _coinsEarnedText.gameObject.SetActive(false);
            }
            SaveManager.Instance.SaveAll();
            PopulateChallenges();
            
        }

        private void PopulateChallenges()
        {
            LevelScriptable currentLevel = GameManager.SelectedLevel;

            foreach(ChallengeScriptable challenge in currentLevel.Challenges)
            {
                if(GameManager.CompletedChallenges.Contains(challenge.ChallengeKey))
                {
                    LevelChallengeUIHandler challengePreview = Instantiate<LevelChallengeUIHandler>(_challengePreviewPrefab, _challengesContainer);
                    challengePreview.UpdateInfo(challenge);
                }
            }
        }
    }
}
