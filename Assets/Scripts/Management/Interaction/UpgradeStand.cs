using UnityEngine;

namespace RPG.Management.Interaction
{
    public class UpgradeStand : MonoBehaviour, IInteractable
    {
        [SerializeField] private RectTransform _progressionCanvas;

        public void Interact()
        {
            OpenUpgradePanel();
        }

        private void OpenUpgradePanel()
        {
            _progressionCanvas.gameObject.SetActive(true);
            ActionsManager.Instance.OnUpgradePanelOpen?.Invoke();

        }

        public void CloseUpgradePanel()
        {
            _progressionCanvas.gameObject.SetActive(false);
            ActionsManager.Instance.OnUpgradePanelClose?.Invoke();
        }
    }
}
