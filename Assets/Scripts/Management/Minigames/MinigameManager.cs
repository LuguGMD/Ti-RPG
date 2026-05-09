using UnityEngine;

namespace RPG.Management.Minigames
{
    public abstract class MinigameManager : MonoBehaviour
    {
        private int _totalHits;
        private int _totalPerfectHits;
        private int _totalMisses;
        private int _totalHitsRecord;

        private int _currentCombo;
        private int _currentPerfectCombo;
        private int _currentComboDuration;

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
    }
}
