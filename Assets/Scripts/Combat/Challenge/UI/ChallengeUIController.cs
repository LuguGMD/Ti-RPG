using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Challenge.UI
{
    public class ChallengeUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform _progressPanel;
        [SerializeField] private ChallengeProgressPanel _challengeProgressPanel;

        private List<ChallengeHandler> _challengeProgressUpdateQueue = new List<ChallengeHandler>();
        private Coroutine _progressUpdateCoroutine;
        private const float PROGRESS_UPDATE_COOLDOWN = 1f;
        private const float PROGRESS_UPDATE_STAY_DURATION = 2f;
        private const float PROGRESS_UPDATE_APPEAR_DURATION = 0.5f;
        
        private void OnEnable()
        {
            ActionsManager.Instance.OnChallengeProgressChanged += AddChallengeProgressUpdate;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnChallengeProgressChanged -= AddChallengeProgressUpdate;
        }

        private void AddChallengeProgressUpdate(ChallengeHandler challenge)
        {
            if(_challengeProgressUpdateQueue.Contains(challenge))
            {
                _challengeProgressUpdateQueue.Remove(challenge);
            }

            _challengeProgressUpdateQueue.Add(challenge);

            if (_progressUpdateCoroutine == null)
            {
                _progressUpdateCoroutine = StartCoroutine(ShowProgressUpdateCoroutine());
            }
        }

        private void RemoveChallengeProgressUpdate(ChallengeHandler challenge)
        {
            if (_challengeProgressUpdateQueue.Contains(challenge))
            {
                _challengeProgressUpdateQueue.Remove(challenge);
            }
        }

        private IEnumerator ShowProgressUpdateCoroutine()
        {
            float progressPanelWidth = _progressPanel.rect.width;
            float progressPanelYPos = _progressPanel.rect.y;
            while(_challengeProgressUpdateQueue.Count > 0)
            {
                _challengeProgressPanel.UpdateInfo(_challengeProgressUpdateQueue[0]);
                _progressPanel.DOAnchorPosX(0, PROGRESS_UPDATE_APPEAR_DURATION).From(new Vector2(progressPanelWidth, progressPanelYPos));
                RemoveChallengeProgressUpdate(_challengeProgressUpdateQueue[0]);
                yield return new WaitForSeconds(PROGRESS_UPDATE_STAY_DURATION + PROGRESS_UPDATE_APPEAR_DURATION);
                _progressPanel.DOAnchorPosX(progressPanelWidth, PROGRESS_UPDATE_APPEAR_DURATION).From(new Vector2(0, progressPanelYPos));
                yield return new WaitForSeconds(PROGRESS_UPDATE_APPEAR_DURATION + PROGRESS_UPDATE_COOLDOWN);
            }

            _progressUpdateCoroutine = null;
        }
    }
}
