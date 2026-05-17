using TMPro;
using UnityEngine;

namespace RPG.Management.Progression
{
    public class ProgressionUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsText;

        private void OnEnable()
        {
            ActionsManager.Instance.OnUpgradePanelOpen += UpdateUI;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnUpgradePanelOpen -= UpdateUI;
        }

        private void UpdateUI()
        {
            _coinsText.text = GameManager.Coins.ToString() + "$";
        }
    }
}
