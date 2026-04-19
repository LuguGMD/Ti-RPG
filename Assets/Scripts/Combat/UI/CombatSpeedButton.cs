using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    [RequireComponent(typeof(Button))]
    public class CombatSpeedButton : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            UpdateText();
        }

        private void OnClick()
        {
            CombatManager.Instance.PassCombatSpeedTier();
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = $"x{CombatManager.CombatSpeed}";
        }
    }
}
