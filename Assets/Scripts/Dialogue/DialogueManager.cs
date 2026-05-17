using DG.Tweening;
using Lugu.Singleton;
using RPG.Input;
using RPG.Management;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace RPG.Dialogue
{
    public class DialogueManager : SingletonMono<DialogueManager>
    {
        private PlayerInput player;
        private DialogueCharacter currentCharacter;
        private Tween lineProgressTween;
        private float lineProgress;
        private TextProgress textProgress;

        [Header("UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterIcon;
        [SerializeField] private List<DialogueIcon> dialogueIcons;
        private Dictionary<string, DialogueIcon> dialogueIconsDictionary = new Dictionary<string, DialogueIcon>();

        private const float CHAR_DURATION = 0.08f;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            player = GameObject.FindAnyObjectByType<PlayerInput>();

            foreach (DialogueIcon icon in dialogueIcons)
            {
                dialogueIconsDictionary[icon.Key] = icon;
            }
        }

        public Dialogue Current { get; private set; }

        public void Next()
        {
            lineProgress = 0;

            string entryDialogueKey = currentCharacter.EntryDialogueKey;
            StringTable table = currentCharacter.StringTable.GetTable();

            Current = Current == null ?
                Dialogue.Get(table, entryDialogueKey)
              : Current.GetNext();

            Display();

            if (Current != null)
            {
                float duration = CHAR_DURATION * Current.TextLocalized.Length;
                lineProgressTween = DOTween.To(() => lineProgress, x => lineProgress = x, 1f, duration);

                textProgress = new TextProgress(Current.TextLocalized);
            }
        }

        public void SetDialogueCharacter(DialogueCharacter character)
        {
            currentCharacter = character;
            dialoguePanel.SetActive(true);
            ActionsManager.Instance.OnDialogueStart?.Invoke();
            Next();
        }

        #region Examples

        protected void Start()
        {
            //player.Actions.Interact.OnStart(OnDialogueInput);
            player.Actions.Jump.OnStart(OnDialogueInput);
        }

        public void OnDialogueInput()
        {
            if (!dialoguePanel.activeSelf) return;

            if (lineProgressTween == null || lineProgress == 1)
            {
                Next();
            }
            else
            {
                lineProgressTween.Kill(true);
                lineProgressTween = null;
            }
        }

        protected void Update()
        {
            if (Current != null && textProgress != null)
            {
                textProgress.SetProgress(lineProgress);
                dialogueText.text = textProgress.ToString();
            }
        }

        private void Display()
        {
            if(Current != null)
            {
                DialogueIcon dialogueIcon = dialogueIconsDictionary[Current.SpriteKey];
                characterIcon.sprite = dialogueIcon.Icon;
                characterNameText.text = dialogueIcon.Name;
            }

            switch (Current)
            {
                case Dialogue.WithChoice:
                    Dialogue.WithChoice current = Current as Dialogue.WithChoice;
                    /*Debug.Log(
                        Current.Key + '\n' +
                        Current.TextLocalized
                    );*/
                    foreach (Dialogue.Choice choice in current.Choices)
                    {
                        /*Debug.Log(
                            choice.Key + '\n' +
                            choice.TextLocalized
                        );*/
                    }
                    break;

                case Dialogue:
                    /*Debug.Log(
                        Current.Key + '\n' +
                        Current.TextLocalized
                    );*/
                    break;

                default:
                    EndDialogue();
                    break;
            }
        }

        private void EndDialogue()
        {
            dialoguePanel.SetActive(false);
            ActionsManager.Instance.OnDialogueEnd?.Invoke();
            //TO DO - REMOVER DEPOIS
            if(currentCharacter.CharacterInfo != null)
                ActionsManager.Instance.OnCharacterMinigameSelected?.Invoke(currentCharacter.CharacterInfo);
        }

        #endregion
    }
}
