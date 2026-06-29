using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tutorial.Handlers
{
    public abstract class TutorialHandler : MonoBehaviour
    {
        protected bool _isCompleted = false;
        [SerializeField] protected string _tutorialKey = "TutorialDefaultKey";
        protected TutorialUIHandler _uiHandler;
        [SerializeField] private string _titleString;
        [TextArea] [SerializeField] private string _descriptionString;

        #region Properties

        public bool IsCompleted { get { return _isCompleted; } }
        public string TutorialKey {  get { return _tutorialKey; } }

        #endregion

        protected virtual void Awake()
        {
            _uiHandler = GetComponent<TutorialUIHandler>();
        }

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

        public virtual void Show()
        {
            if (TutorialManager.Instance == null) return;

            if (!TutorialManager.Instance.IsTutorialComplete(_tutorialKey))
            {

                _uiHandler.Show(_titleString, _descriptionString);
            }
        }

        public void Complete()
        {
            _isCompleted = true;
            TutorialManager.Instance.SetTutorialCompletion(_tutorialKey, true);
            enabled = false;
        }
    }
}
