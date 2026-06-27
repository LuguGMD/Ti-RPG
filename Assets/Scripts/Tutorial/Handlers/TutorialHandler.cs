using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public abstract class TutorialHandler : MonoBehaviour
    {
        protected bool _isCompleted = false;
        protected string _tutorialKey = "TutorialDefaultKey";

        #region Properties

        public bool IsCompleted { get { return _isCompleted; } }
        public string TutorialKey {  get { return _tutorialKey; } }

        #endregion

        protected virtual void OnEnable()
        {
            SubscribeCallbacks();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeCallbacks();
        }

        public abstract void SubscribeCallbacks();
        public abstract void UnsubscribeCallbacks();

        public abstract void Show();
        public abstract void Hide();
        protected void Complete()
        {
            _isCompleted = true;
            TutorialManager.Instance.SetTutorialCompletion(_tutorialKey, true);
            enabled = false;
        }
    }
}
