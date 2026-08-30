using Lugu.Singleton;
using UnityEngine;

namespace RPG.Management
{
    public class ManagementManager : SingletonMono<ManagementManager>
    {
        private bool _isInteractionRunning = false;

        #region Properties

        public static bool IsInteractionRunning { get { return Instance._isInteractionRunning; } }

        #endregion

        private void OnEnable()
        {
            ActionsManager.Instance.OnDialogueStart += StartInteraction;
            ActionsManager.Instance.OnDialogueEnd += EndInteraction;
            ActionsManager.Instance.OnUpgradePanelOpen += StartInteraction;
            ActionsManager.Instance.OnUpgradePanelClose += EndInteraction;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnDialogueStart -= StartInteraction;
            ActionsManager.Instance.OnDialogueEnd -= EndInteraction;
            ActionsManager.Instance.OnUpgradePanelOpen -= StartInteraction;
            ActionsManager.Instance.OnUpgradePanelClose -= EndInteraction;
        }

        private void StartInteraction()
        {
            _isInteractionRunning = true;
        }

        private void EndInteraction()
        {
            _isInteractionRunning = false;
        }
    }
}
