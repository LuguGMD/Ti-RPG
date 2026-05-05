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

        protected void AddProgress(float progress)
        {
            SetProgress(_currentValue + progress);
        }

        protected void SetProgress(float progress)
        {

            if (!IsComplete)
            {
                _currentValue = progress;
                UpdateProgress();

                ActionsManager.Instance.OnChallengeProgressChanged?.Invoke(this);
            }
        }

        protected void ResetProgress()
        {
            if (!IsComplete)
            {
                _currentValue = 0;
                UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            _currentValue = Mathf.Clamp(_currentValue, 0f, _info.TargetValue);
            CheckCompletion();
        }

        private void CheckCompletion()
        {
            if(_currentValue >= _info.TargetValue)
            {
                Complete();
            }
        }
        private void Complete()
        {
            ActionsManager.Instance.OnChallengeCompleted?.Invoke(this);
        }
        public abstract void SubscribeCallbacks();
        public abstract void UnsubscribeCallbacks();
        public void SetInfo(ChallengeScriptable info)
        {
            _info = info;
        }
    }
}
