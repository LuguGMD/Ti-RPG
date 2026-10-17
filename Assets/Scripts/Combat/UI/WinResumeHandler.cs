using DG.Tweening;
using RPG.Combat.Challenge;
using RPG.Level;
using RPG.Save;
using RPG.UI;
using System.Collections;
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
        [SerializeField] private TextMeshProUGUI _titleText;

        private void Start()
        {
            StartCoroutine(PanelCoroutine());
        }

        private IEnumerator PanelCoroutine()
        {
            LevelScriptable currentLevel = GameManager.SelectedLevel;
            if (!GameManager.CompletedLevels.Contains(currentLevel.LevelKey))
            {
                GameManager.Instance.CompleteLevel(currentLevel.LevelKey);
            }

            _challengesContainer.gameObject.SetActive(false);

            yield return StartCoroutine(UIAnimations.RevealRectTransformScale(_titleText.rectTransform));

            _challengesContainer.gameObject.SetActive(true);

            yield return StartCoroutine(UIAnimations.RevealRectTransformScale(_challengesContainer));

            SaveManager.Instance.SaveAll();
            yield return StartCoroutine(PopulateChallenges());
        }

        

        private IEnumerator PopulateChallenges()
        {
            LevelScriptable currentLevel = GameManager.SelectedLevel;

            foreach(ChallengeScriptable challenge in currentLevel.Challenges)
            {
                LevelChallengeUIHandler challengePreview = Instantiate<LevelChallengeUIHandler>(_challengePreviewPrefab, _challengesContainer);
                challengePreview.UpdateInfo(challenge);
                float placeDuration = 1f;
                RectTransform rect = challengePreview.GetComponent<RectTransform>();
                rect.DOScale(Vector3.one, placeDuration).From(Vector3.one * 1.2f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    Vector2 anchorPos = rect.anchoredPosition;
                    rect.DOShakeAnchorPos(0.25f, 5).OnComplete(() =>
                    {
                        rect.anchoredPosition = anchorPos;
                    });
                });
                yield return new WaitForSeconds(placeDuration*1.5f);
                if(GameManager.CompletedChallenges.Contains(challenge.ChallengeKey))
                {
                   // TO DO tocar efeito sonoro de ganho de moeda
                }
            }
        }
    }
}
