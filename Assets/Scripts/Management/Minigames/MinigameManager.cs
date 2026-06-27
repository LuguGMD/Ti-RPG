using Lugu.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Management.Minigames
{
    public class MinigameManager : SingletonMono<MinigameManager>
    {
        private int _totalHits;
        private int _totalPerfectHits;
        private int _totalMisses;
        private int _totalHitsHighscore;

        private int _currentCombo;
        private int _currentPerfectCombo;
        private int _currentComboDuration;

        private bool _isPaused = false;

        [SerializeField] private List<MinigameChallenge> _challenges;
        private int _currentChallengeIndex = 0;
        [SerializeField] private List<int> _tiersTreshold;
        int _currentTier = 0;

        #region Properties

        public static int TotalHits { get { return Instance._totalHits; } }
        public static int TotalPerfectHits { get { return Instance._totalPerfectHits; } }
        public static int TotalMisses {  get { return Instance._totalMisses; } }
        public static int TotalHitsHighscore { get { return Instance._totalHitsHighscore; } }

        public static int CurrentCombo { get { return Instance._currentCombo; } }
        public static int CurrentPerfectCombo { get { return Instance._currentPerfectCombo; } }
        public static int CurrentComboDuration {  get { return Instance._currentComboDuration; } }

        public static bool IsPaused { get { return Instance._isPaused; } }
        public static int CurrentChallengeIndex { get { return Instance._currentChallengeIndex; } }

        public static int CurrentTier {  get { return Instance._currentTier; } }

        #endregion

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnMinigameHit += OnHit;
            ActionsManager.Instance.OnMinigameHit += ResetPerfectCombo;
            ActionsManager.Instance.OnMinigamePerfectHit += OnPerfectHit;
            ActionsManager.Instance.OnMinigameMiss += OnMiss;
            ActionsManager.Instance.OnMinigameChallengeCompleted += OnChallengeCompleted;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMinigameHit -= OnHit;
            ActionsManager.Instance.OnMinigameHit -= ResetPerfectCombo;
            ActionsManager.Instance.OnMinigamePerfectHit -= OnPerfectHit;
            ActionsManager.Instance.OnMinigameMiss -= OnMiss;
            ActionsManager.Instance.OnMinigameChallengeCompleted -= OnChallengeCompleted;
        }

        private void Init()
        {
            _totalHits = 0;
            _totalPerfectHits = 0;
            _totalMisses = 0;
            ResetCombo();
            ResetPerfectCombo();
            InitCurrentChallenge();

            ActionsManager.Instance.OnMinigameStart?.Invoke();
        }

        public void EndMinigame()
        {
            ActionsManager.Instance.OnMinigameEnd?.Invoke();

            //TO DO adicionar uma condição depois
            ActionsManager.Instance.OnCharacterMotivated?.Invoke(GameManager.SelectedCharacterMinigame);
        }

        private void ResetPerfectCombo()
        {
            _currentPerfectCombo = 0;
        }

        private void ResetCombo()
        {
            _currentCombo = 0;
            _currentComboDuration = 0;
            ResetPerfectCombo();
        }

        private void OnHit()
        {
            _totalHits++;
            _currentCombo++;

            ChangeTier();

            ActionsManager.Instance.OnMinigameValuesUpdated?.Invoke();
        }

        private void OnPerfectHit()
        {
            OnHit();
            _totalPerfectHits++;
            _currentPerfectCombo++;
        }

        private void OnMiss()
        {
            _totalMisses++;
            ResetPerfectCombo();
            ResetCombo();

            ChangeTier();

            ActionsManager.Instance.OnMinigameValuesUpdated?.Invoke();
        }

        private void ChangeTier()
        {
            int previousTier = _currentTier;

            for(int i =0; i < _tiersTreshold.Count; i++) 
            {
                if (_currentCombo > _tiersTreshold[i]) continue;
                else
                {
                    _currentTier = i;

                    if (previousTier != _currentTier)
                    {
                        ActionsManager.Instance.OnMinigameTierChanged?.Invoke();
                    }

                    return;
                }
            }
        }

        public void TogglePause(bool doPaused)
        {
            _isPaused = doPaused;
        }

        private void OnChallengeCompleted()
        {
            _challenges[_currentChallengeIndex].RemoveListeners();

            _currentChallengeIndex++;
            InitCurrentChallenge();
        }

        private void InitCurrentChallenge()
        {
            if (_currentChallengeIndex < _challenges.Count)
            {
                _challenges[_currentChallengeIndex].AddListeners();
            }
        }
    }
}
