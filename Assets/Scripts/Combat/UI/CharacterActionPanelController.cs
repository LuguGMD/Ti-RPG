using UnityEngine;
using TMPro;
using RPG.Combat.Actions;
using System.Collections.Generic;

namespace RPG.Combat.UI
{
    public class CharacterActionPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _characterNameDisplay;
        [SerializeField] private TextMeshProUGUI _actionDescriptionDisplay;
        [SerializeField] private ActionButtonHandler _actionButtonPrefab;
        [SerializeField] private RectTransform _actionsPanel;
        private List<ActionButtonHandler> _actionButtons = new List<ActionButtonHandler>();

        private CharacterController _selectedCharacter;

        private void Start()
        {
            HidePanel();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterSelected += OnCharacterSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterSelected -= OnCharacterSelected;
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
            int count = Mathf.Max(actions.Length, _actionButtons.Count);

            for (int i = 0; i < count; i++)
            {
                if (i >= _actionButtons.Count)
                {
                    InstantiateActionButton();
                }

                if(i < _actionButtons.Count)
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

        private void InstantiateActionButton()
        {
            ActionButtonHandler actionButton = Instantiate<ActionButtonHandler>(_actionButtonPrefab, _actionsPanel);
            _actionButtons.Add(actionButton);
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
            _panel.SetActive(true);
        }

        private void HidePanel()
        {
            _panel.SetActive(false);
        }
    }
}
