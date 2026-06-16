using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Dialogue
{
    public class DialogueOptionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _optionText;
        private Button _button;
        int _optionIndex;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Init(Dialogue choice, int optionIndex)
        {
            _optionText.text = choice.LocalizedText;
            _optionIndex = optionIndex;
            _button.onClick.AddListener(() =>
            {
                ((DialogueWithChoice)DialogueController.Instance.CurrentDialogue).Select(optionIndex);
                DialogueController.Instance.HandleInput();
            });
        }
    }
}
