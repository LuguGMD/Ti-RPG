using Lugu.Singleton;
using RPG.Combat;
using RPG.Combat.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level
{
    public class LevelSelectUIController : SingletonMono<LevelSelectUIController>
    {
        [SerializeField] private RectTransform _mainPanel;
        private LevelScriptable _selectedLevel;

        [SerializeField] private LevelChallengeUIHandler _challengePanelPrefab;
        [SerializeField] private RectTransform _challengesContainer;
        private List<LevelChallengeUIHandler> _challengePanels = new List<LevelChallengeUIHandler>();

        [SerializeField] private CharacterOptionButton _characterOptionPrefab;
        [SerializeField] private RectTransform _characterOptionsContainer;
        private List<CharacterOptionButton> _characterOptions = new List<CharacterOptionButton>();

        [SerializeField] private RectTransform _partyMemberModelPreview;

        [SerializeField] private RectTransform _partyMemberDescriptionPanel;
        [SerializeField] private TextMeshProUGUI _partyMemberNameText;
        [SerializeField] private TextMeshProUGUI _partyMemberDescriptionText;
        [SerializeField] private Slider _motivationBarSlider;
        [SerializeField] private TextMeshProUGUI _levelNameText;
        [SerializeField] private TextMeshProUGUI _selectedCharactersCountText;
        [SerializeField] private Transform[] _partyMemberModelPreviews;

        [SerializeField] private Button _confirmPlayButton;

        private int _selectedPartyIndex = 0;

        private static bool _isActive = false;

        #region Properties

        public static bool IsActive { get { return _isActive; } }

        #endregion

        private void Start()
        {
            _confirmPlayButton.onClick.AddListener(ConfirmButton);
            CreateCharacterOptions();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnLevelSelected += OnLevelSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnLevelSelected -= OnLevelSelected;
            _isActive = false;
        }

        private void OnLevelSelected(LevelScriptable selectedLevel)
        {
            _mainPanel.gameObject.SetActive(true);
            _isActive = true;

            _selectedLevel = selectedLevel;
            UpdateChallenges();
            UpdateCharacterOptions();
            UpdateCharacterCount();

            _levelNameText.text = _selectedLevel.levelName;
            UpdateCharacterInfo(GameManager.CurrentParty[0]);
        }

        private void UpdateChallenges()
        {
            int count = Mathf.Max(_selectedLevel.Challenges.Length, _challengePanels.Count);
            for (int i = 0; i < count; i++)
            {
                if (i >= _challengePanels.Count)
                {
                    CreateChallenge();
                }

                _challengePanels[i].gameObject.SetActive(i < _selectedLevel.Challenges.Length);
                if (i < _selectedLevel.Challenges.Length)
                {
                    _challengePanels[i].UpdateInfo(_selectedLevel.Challenges[i]);
                }
            }

        }

        private void CreateChallenge()
        {
            LevelChallengeUIHandler challengePanel = Instantiate<LevelChallengeUIHandler>(_challengePanelPrefab, _challengesContainer);
            _challengePanels.Add(challengePanel);
        }

        private void UpdateCharacterOptions()
        {
            foreach (CharacterOptionButton characterOption in _characterOptions)
            {
                characterOption.UpdateVisual(GameManager.CurrentParty.Contains(characterOption.Character));
            }
        }

        private void CreateCharacterOptions()
        {
            for (int i = 0; i < GameManager.AvailableCharacters.Length; i++)
            {
                CharacterOptionButton characterOptionButton = Instantiate<CharacterOptionButton>(_characterOptionPrefab, _characterOptionsContainer);
                characterOptionButton.UpdateInfo(GameManager.AvailableCharacters[i]);
                _characterOptions.Add(characterOptionButton);
            }
        }

        public void UpdateCharacterInfo(CharacterScriptable character)
        {
            _partyMemberDescriptionPanel.gameObject.SetActive(character != null);
            if (character == null) return;

            _partyMemberNameText.text = character.EntityName;
            _partyMemberDescriptionText.text = character.EntityDescription;

            float motivationValue = character.Motivation / CombatConstants.MAX_MOTIVATION_APRESENTADOR;

            _motivationBarSlider.value = motivationValue;

            if (character.PreviewModelPrefab != null)
            {
                foreach (Transform child in _partyMemberModelPreview)
                {
                    Destroy(child.gameObject);
                }
                GameObject characterModel = Instantiate(character.PreviewModelPrefab.gameObject, _partyMemberModelPreview);
                characterModel.transform.localPosition = Vector3.zero;
                characterModel.transform.localRotation = Quaternion.identity;
            }
        }

        public void RemovePartyMember(CharacterScriptable character)
        {
            int index = Array.IndexOf(GameManager.CurrentParty, character);
            if (index >= 0)
            {
                _selectedPartyIndex = index;
                GameManager.CurrentParty[index] = null;
            }

            if (_partyMemberModelPreviews[index].childCount > 0)
            {
                foreach (Transform child in _partyMemberModelPreviews[index])
                {
                    Destroy(child.gameObject);
                }
            }

            UpdateCharacterOptions();
            UpdateCharacterCount();
        }

        public void AddPartyMember(CharacterScriptable character)
        {
            GameManager.CurrentParty[_selectedPartyIndex] = character;

            UpdateCharacterOptions();
            UpdateCharacterCount();

            foreach (Transform child in _partyMemberModelPreviews[_selectedPartyIndex])
            {
                Destroy(child.gameObject);
            }
            GameObject characterModel = Instantiate(character.PreviewModelPrefab.gameObject, _partyMemberModelPreviews[_selectedPartyIndex]);
            characterModel.transform.localPosition = Vector3.zero;
            characterModel.transform.localRotation = Quaternion.identity;

            for (int i = 0; i < GameManager.CurrentParty.Length; i++)
            {
                if (GameManager.CurrentParty[i] == null)
                {
                    _selectedPartyIndex = i;
                    break;
                }
            }
        }

        private void UpdateCharacterCount()
        {
            _confirmPlayButton.interactable = GameManager.CurrentParty.Count(c => c != null) == CombatConstants.MAX_CHARACTERS_COUNT;
            _selectedCharactersCountText.text = GameManager.CurrentParty.Count(c => c != null) + "/" + CombatConstants.MAX_CHARACTERS_COUNT;
        }

        public void ClosePanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _isActive = false;
        }

        public void ConfirmButton()
        {
            bool isPartyValid = true;

            foreach (CharacterScriptable character in GameManager.CurrentParty)
            {
                if (character == null)
                    isPartyValid = false;
            }

            if (isPartyValid)
                GameManager.ChangeScene(ScenesEnum.Combat);
            else
                ActionsManager.Instance.OnError?.Invoke();
        }
    }
}
