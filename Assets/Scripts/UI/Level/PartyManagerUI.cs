using RPG.Combat;
using RPG.Level;
using RPG.Level.Challenge;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class PartyManagerUI : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameManager gameManager;

        [Header("Level UI")]
        [SerializeField] private LevelScriptable levelData;

        [Header("Challenge UI")]
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private List<TextMeshProUGUI> challengeTitleText;
        [SerializeField] private List<TextMeshProUGUI> challengeDescriptionText;
        [SerializeField] private List<TextMeshProUGUI> challengeRewardText;

        [Header("Party UI")]
        [SerializeField] private List<PartyCharacterButton> partyButtonCharacters;
        [SerializeField] private List<Image> partyMemberIcon;
        [SerializeField] private TextMeshProUGUI partyMemberText;
        [SerializeField] private Slider motivationBarSlider;

        [Header("Data")]
        [SerializeField] private List<CharacterScriptable> availableCharacters;

        private int selectedPartyIndex = -1;

        private void Awake()
        {
            LevelText.text = levelData.name;

            for (int i = 0; i < levelData.Challenges.Length; i++)
            {
                ChallengeScriptable challenge = levelData.Challenges[i];
                challengeTitleText[i].text = challenge.ChallengeName;
                challengeDescriptionText[i].text = challenge.Description;
                challengeRewardText[i].text = challenge.CoinsReward.ToString() + " moedas";
            }

            for (int i = 0; i < GameManager.CurrentParty.Length; i++)
            {
                if (GameManager.CurrentParty[i] != null)
                {
                    partyButtonCharacters[i].character = GameManager.CurrentParty[i];
                    UpdatePartyMemberIcon(i);
                }
            }
        }

        public void SelectPartyMember(int index)
        {
            selectedPartyIndex = index;
        }

        public void ReplaceCharacter(CharacterScriptable character)
        {
            GameManager.CurrentParty[selectedPartyIndex] = character;
            partyButtonCharacters[selectedPartyIndex].character = character;

            UpdatePartyMemberIcon(selectedPartyIndex);
        }

        private void UpdatePartyMemberIcon(int partyIndex)
        { 
            partyMemberIcon[partyIndex].sprite = GameManager.CurrentParty[partyIndex].Icon;
        }

        public void UpdateCharacterInfo(CharacterScriptable character)
        {
            partyMemberText.text = character.EntityName;
            motivationBarSlider.value = character.Motivation;
        }
    }
}
