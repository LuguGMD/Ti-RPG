using UnityEngine;

namespace RPG.Management.Minigames
{
    [System.Serializable]
    public class MinigameChallenge
    {
        [SerializeField] private MinigameChallengeEnum _challengeType;
        [Min(1)] [SerializeField] private int _challengeGoal = 1;
        private int _challengeProgress = 0;
        private bool _isComplete = false;

        #region Properties

        public MinigameChallengeEnum ChallengeType { get { return _challengeType; } }
        public int ChallengeGoal { get { return _challengeGoal; } }
        public int ChallengeProgress { get { return _challengeProgress; } }
        public bool IsComplete { get { return _isComplete; } }

        #endregion

        public void AddListeners()
        {
            switch (ChallengeType)
            {
                case MinigameChallengeEnum.CurrentCombo:
                    ActionsManager.Instance.OnMinigameHit += AddProgress;
                    ActionsManager.Instance.OnMinigamePerfectHit += AddProgress;
                    ActionsManager.Instance.OnMinigameMiss += ResetProgress;
                    break;
                case MinigameChallengeEnum.CurrentPerfectCombo:
                    ActionsManager.Instance.OnMinigamePerfectHit += AddProgress;
                    ActionsManager.Instance.OnMinigameHit += ResetProgress;
                    ActionsManager.Instance.OnMinigameMiss += ResetProgress;
                    break;
                case MinigameChallengeEnum.TotalPerfects:
                    ActionsManager.Instance.OnMinigamePerfectHit += AddProgress;
                    break;
                case MinigameChallengeEnum.TotalHits:
                    ActionsManager.Instance.OnMinigameHit += AddProgress;
                    ActionsManager.Instance.OnMinigamePerfectHit += AddProgress;
                    break;
                case MinigameChallengeEnum.Duration:
                    Debug.LogWarning("Desafio de Minigame não implementado");
                    ActionsManager.Instance.OnMinigameHit += ResetProgress;
                    break;
            }

            ActionsManager.Instance.OnMinigameChallengeUpdated?.Invoke(this);
        }

        public void RemoveListeners()
        {
            switch (ChallengeType)
            {
                case MinigameChallengeEnum.CurrentCombo:
                    ActionsManager.Instance.OnMinigameHit -= AddProgress;
                    ActionsManager.Instance.OnMinigamePerfectHit -= AddProgress;
                    ActionsManager.Instance.OnMinigameMiss -= ResetProgress;
                    break;
                case MinigameChallengeEnum.CurrentPerfectCombo:
                    ActionsManager.Instance.OnMinigamePerfectHit -= AddProgress;
                    ActionsManager.Instance.OnMinigameHit -= ResetProgress;
                    ActionsManager.Instance.OnMinigameMiss -= ResetProgress;
                    break;
                case MinigameChallengeEnum.TotalPerfects:
                    ActionsManager.Instance.OnMinigamePerfectHit -= AddProgress;
                    break;
                case MinigameChallengeEnum.TotalHits:
                    ActionsManager.Instance.OnMinigameHit -= AddProgress;
                    ActionsManager.Instance.OnMinigamePerfectHit -= AddProgress;
                    break;
                case MinigameChallengeEnum.Duration:
                    Debug.LogWarning("Desafio de Minigame não implementado");
                    ActionsManager.Instance.OnMinigameHit -= ResetProgress;
                    break;
            }
        }

        private void AddProgress()
        {
            _challengeProgress++;
            UpdateProgress();
        }

        private void ResetProgress()
        {
            _challengeProgress = 0;
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            ActionsManager.Instance.OnMinigameChallengeUpdated?.Invoke(this);
            _challengeProgress = Mathf.Clamp(_challengeProgress, 0, _challengeGoal);
            if(!_isComplete && _challengeProgress >= _challengeGoal)
            {
                CompleteChallenge();
            }
        }

        private void CompleteChallenge()
        {
            ActionsManager.Instance.OnMinigameChallengeCompleted?.Invoke();
            _isComplete = true;
        }
    }
}
