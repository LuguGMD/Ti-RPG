using UnityEngine;

namespace RPG.Management.Interaction
{
    public class UpgradeStand : MonoBehaviour, IInteractable
    {
        [SerializeField] private RectTransform _progressionCanvas;

        public void Interact()
        {
            _progressionCanvas.gameObject.SetActive(true);
        }
    }
}
