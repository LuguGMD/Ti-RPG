using UnityEngine;

namespace RPG.Level.Challenge
{
    public abstract class ChallengeHandler : MonoBehaviour
    {
        private float _currentValue;
        private ChallengeScriptable _info;

        #region Properties

        public float CurrentValue
        {
            get { return _currentValue; }
        }

        public ChallengeScriptable Info
        {
            get { return _info; }
        }

        public bool IsComplete
        {
            get { return _currentValue >= _info.TargetValue; }
        }

        #endregion

        private void AddProgress(float progress)
        {
            SetProgress(_currentValue + progress);
        }

        private void SetProgress(float progress)
        {
            if (!IsComplete)
            {
                _currentValue = progress;
                UpdateProgress();
                //TO DO - ActionsManager.OnChallengeProgressChanged?.Invoke(ChallengeHandler);
            }
        }

        private void ResetProgress()
        {
            _currentValue = 0;
        }

        private void UpdateProgress()
        {
            _currentValue = Mathf.Clamp(_currentValue, 0f, _info.TargetValue);
        }


        public abstract void SubscribeCallbacks();
        public abstract void UnsubscribeCallbacks();
    }
}
