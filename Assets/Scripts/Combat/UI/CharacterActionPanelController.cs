using UnityEngine;
using TMPro;
using RPG.Combat.Actions;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterActionPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _characterNameText;
        [SerializeField] private RectTransform _actionsDescriptionPanel;
        [SerializeField] private TextMeshProUGUI _actionDescriptionText;
        [SerializeField] private TextMeshProUGUI _spotlightDescriptionText;
        [SerializeField] private ActionButtonHandler _actionButtonPrefab;
        [SerializeField] private RectTransform _actionsPanel;
        [SerializeField] private Button _cancelButton;
        private List<ActionButtonHandler> _actionButtons = new List<ActionButtonHandler>();

        private CharacterController _selectedCharacter;

        private void Start()
        {
            HidePanel();
            _cancelButton.onClick.AddListener(HidePanel);
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnEntitySelected += OnEntitySelected;
            ActionsManager.Instance.OnCharacterSelected += OnCharacterSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnEntitySelected -= OnEntitySelected;
            ActionsManager.Instance.OnCharacterSelected -= OnCharacterSelected;
            HidePanel();
        }

        private void OnEntitySelected(EntityController entity)
        {
            if(entity != _selectedCharacter)
            {
                HidePanel();
            }
        }

        private void OnCharacterSelected(CharacterController character)
        {
            _selectedCharacter = character;
            PopulateActionButtons(character);
            ShowPanel();
        }

        private void PopulateActionButtons(CharacterController character)
        {
            List<CombatAction> actions = character.Actions;
            int count = Mathf.Max(actions.Count, _actionButtons.Count);

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

            _characterNameText.text = character.CharacterInfo.EntityName;
            _spotlightDescriptionText.text = character.CharacterInfo.SpotlightDescription;
            _actionsDescriptionPanel.gameObject.SetActive(false);
        }

        private void InstantiateActionButton()
        {
            ActionButtonHandler actionButton = Instantiate<ActionButtonHandler>(_actionButtonPrefab, _actionsPanel);
            _actionButtons.Add(actionButton);
        }

        public void OnActionButtonClicked()
        {
            HidePanel();
        }

        public void OnActionButtonHovered(CombatAction action)
        {
            _actionDescriptionText.text = action.ActionDescription;
            if (action.ActionDescription != "")
            {
                _actionsDescriptionPanel.gameObject.SetActive(true);
            }
        }

        public void OnActionButtonExited()
        {
            _actionsDescriptionPanel.gameObject.SetActive(false);
        }

        public void OnActionSelected(int actionIndex)
        {
            if (_selectedCharacter != null && _selectedCharacter.SelectedActionIndex != actionIndex)
            {
                _selectedCharacter.SelectAction(actionIndex);
            }
        }

        private void ShowPanel()
        {
            CombatUIManager.Instance.ChangePanel(_panel);
            ActionsManager.Instance.OnTurnPassed += HidePanel;
            ActionsManager.Instance.OnActionStart += HidePanel;
        }

        private void HidePanel()
        {
            CombatUIManager.Instance.DisablePanel(_panel);
            ActionsManager.Instance.OnTurnPassed -= HidePanel;
            ActionsManager.Instance.OnActionStart -= HidePanel;
        }
    }
}
