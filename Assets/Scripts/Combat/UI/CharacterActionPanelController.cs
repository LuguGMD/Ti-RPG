using UnityEngine;
using TMPro;
using RPG.Combat.Actions;

namespace RPG.Combat.UI
{
    public class CharacterActionPanelController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _characterNameDisplay;
        [SerializeField] private TextMeshProUGUI _actionDescriptionDisplay;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ActionButtonHandler[] _actionButtons;

        private CharacterController _selectedCharacter;

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterClicked += OnCharacterSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterClicked -= OnCharacterSelected;
        }

        private void OnCharacterSelected(CharacterController character)
        {
            _selectedCharacter = character;
            PopulateActionButtons(character);
            ShowPanel();
        }

        private void PopulateActionButtons(CharacterController character)
        {
            CombatAction[] actions = character.CharacterInfo.Actions;

            for (int i = 0; i < _actionButtons.Length; i++)
            {
                if (i < actions.Length)
                {
                    _actionButtons[i].Initialize(i, actions[i], character, this);
                    _actionButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    _actionButtons[i].gameObject.SetActive(false);
                }
            }

            _characterNameDisplay.text = character.CharacterInfo.EntityName;
            _actionDescriptionDisplay.text = string.Empty;
        }

        public void OnActionButtonHovered(CombatAction action)
        {
            _actionDescriptionDisplay.text = action.ActionDescription;
        }

        public void OnActionButtonExited()
        {
            _actionDescriptionDisplay.text = string.Empty;
        }

        public void OnActionSelected(int actionIndex)
        {
            if (_selectedCharacter != null)
            {
                _selectedCharacter.SelectAction(actionIndex);
                HidePanel();
            }
        }

        private void ShowPanel()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        private void HidePanel()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
