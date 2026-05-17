using Lugu.Singleton;
using RPG.Combat;
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

        [SerializeField] private List<PartyCharacterButton> _partyButtonCharacters;

        [SerializeField] private TextMeshProUGUI _partyMemberNameText;
        [SerializeField] private Slider _motivationBarSlider;
        [SerializeField] private RectTransform _characterOptionsPanel;
        [SerializeField] private TextMeshProUGUI _levelNameText;

        private int _selectedPartyIndex = -1;

        private static bool _isActive = false;

        #region Properties

        public static bool IsActive { get { return _isActive; } }

        #endregion

        private void Start()
        {
            CreateCharacterOptions();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnLevelSelected += OnLevelSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnLevelSelected -= OnLevelSelected;
        }

        private void OnLevelSelected(LevelScriptable selectedLevel)
        {
            _mainPanel.gameObject.SetActive(true);
            _isActive = true;

            _selectedLevel = selectedLevel;
            UpdateChallenges();
            UpdateCharacterOptions();
            UpdatePartyOptions();

            _levelNameText.text = _selectedLevel.levelName;
            _characterOptionsPanel.gameObject.SetActive(false);
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
                if(i < _selectedLevel.Challenges.Length)
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
            foreach(CharacterOptionButton characterOption in _characterOptions)
            {
                characterOption.gameObject.SetActive(!GameManager.CurrentParty.Contains(characterOption.Character));
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

        private void UpdatePartyOptions()
        {
            for (int i = 0; i < GameManager.CurrentParty.Length; i++)
            {
                _partyButtonCharacters[i].SetIndex(i);
                _partyButtonCharacters[i].UpdateInfo(GameManager.CurrentParty[i]);
            }
        }

        public void UpdateCharacterInfo(CharacterScriptable character)
        {
            _partyMemberNameText.text = character.EntityName;

            float motivationValue = character.Motivation / CombatConstants.MAX_MOTIVATION_APRESENTADOR;
            motivationValue = GameManager.DefeatedCharacters.Contains(character) ? 0 : motivationValue;

            _motivationBarSlider.value = motivationValue;
        }

        public void SelectPartyMember(int index)
        {
            _selectedPartyIndex = index;
            UpdateCharacterOptions();
            _characterOptionsPanel.gameObject.SetActive(true);
        }

        public void ReplaceCharacter(CharacterScriptable character)
        {
            GameManager.CurrentParty[_selectedPartyIndex] = character;
            _partyButtonCharacters[_selectedPartyIndex].UpdateInfo(character);
            _characterOptionsPanel.gameObject.SetActive(false);
        }

        public void ClosePanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _isActive = false;
        }

        public void ConfirmButton()
        {
            bool isPartyValid = true;

            foreach(CharacterScriptable character in GameManager.CurrentParty)
            {
                if (GameManager.DefeatedCharacters.Contains(character))
                    isPartyValid = false;
            }

            if(isPartyValid)
                GameManager.ChangeScene(ScenesEnum.Combat);
        }
    }
}
