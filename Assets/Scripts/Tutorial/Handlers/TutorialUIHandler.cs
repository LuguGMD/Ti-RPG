using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Tutorial.Handlers
{
    public class TutorialUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialPanel;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _button;
        private TutorialHandler _handler;

        private void Awake()
        {
            _handler = GetComponent<TutorialHandler>();
            _button.onClick.AddListener(Hide);
        }

        public void Show(string title, string description)
        {
            _titleText.text = title;
            _descriptionText.text = description;
            _tutorialPanel.SetActive(true);
        }
        public void Hide()
        {
            _tutorialPanel.SetActive(false);
            _handler.Complete();
        }
    }
}
