using Lugu.Singleton;
using UnityEngine;

namespace RPG.Management.Minigames
{
    public abstract class MinigameManager : SingletonMono<MinigameManager>
    {
        private int _totalHits;
        private int _totalPerfectHits;
        private int _totalMisses;
        private int _totalHitsHighscore;

        private int _currentCombo;
        private int _currentPerfectCombo;
        private int _currentComboDuration;

        private bool _isPaused = false;

        #region Properties

        public static int TotalHits { get { return Instance._totalHits; } }
        public static int TotalPerfectHits { get { return Instance._totalPerfectHits; } }
        public static int TotalMisses {  get { return Instance._totalMisses; } }
        public static int TotalHitsHighscore { get { return Instance._totalHitsHighscore; } }

        public static int CurrentCombo { get { return Instance._currentCombo; } }
        public static int CurrentPerfectCombo { get { return Instance._currentPerfectCombo; } }
        public static int CurrentComboDuration {  get { return Instance._currentComboDuration; } }

        public static bool IsPaused { get { return Instance._isPaused; } }

        #endregion

        private void OnEnable()
        {
            ActionsManager.Instance.OnMinigameHit += OnHit;
            ActionsManager.Instance.OnMinigamePerfectHit += OnPerfectHit;
            ActionsManager.Instance.OnMinigameMiss += OnMiss;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMinigameHit -= OnHit;
            ActionsManager.Instance.OnMinigamePerfectHit -= OnPerfectHit;
            ActionsManager.Instance.OnMinigameMiss -= OnMiss;
        }

        private void Init()
        {
            _totalHits = 0;
            _totalPerfectHits = 0;
            _totalMisses = 0;
            ResetCombo();
            ResetPerfectCombo();
        }

        private void ResetPerfectCombo()
        {
            ResetPerfectCombo();
        }

        private void ResetCombo()
        {
            _currentCombo = 0;
            _currentComboDuration = 0;
        }

        private void OnHit()
        {
            _totalHits++;
            _currentCombo++;
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
        }

        public void TogglePause(bool doPaused)
        {
            _isPaused = doPaused;
        }
    }
}
