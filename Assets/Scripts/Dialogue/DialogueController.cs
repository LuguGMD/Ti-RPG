using System.Collections;
using DG.Tweening;
using Lugu.Singleton;
using RPG.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace RPG.Dialogue
{
    public class DialogueController : SingletonMono<DialogueController>
    {
        [SerializeField] private LocalizedStringTable dialogues;

        private PlayerInput player;

        [Header("UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private DialogueChoiceController choicesPanel;
        [SerializeField] private Image dialogueSprite;
        [SerializeField] private TextMeshProUGUI dialogueTitle;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button skipDialogueButton;

        public bool IsRunning { get { return dialoguePanel.activeSelf; } }

        protected new void Awake()
        {
            base.Awake();
            Dialogue.LocalizationTable = dialogues;
        }

        private void Start()
        {
            player = FindAnyObjectByType<PlayerInput>();
            EnableInput();
        }

        private void EnableInput()
        {
            player.Actions.Interact.OnStart(HandleInput);
            player.Actions.Jump.OnStart(HandleInput);
        }

        private void DisableInput()
        {
            player.Actions.Interact.Remove.OnStart(HandleInput);
            player.Actions.Jump.Remove.OnStart(HandleInput);
        }

        public void HandleInput()
        {
            if (!dialoguePanel.activeSelf) return;

            if (textWriter != null && textWriter.Progress < 1.0f)
            {
                textProgressTween.Kill(true);
            }
            else if (IsDisplayFinished)
            { DialogueNext(); }
        }

        protected void OnEnable()
        {
            if (!didStart) return;
            EnableInput();
        }

        protected void OnDisable()
        {
            if (!didStart) return;
            DisableInput();
        }

        private const float CHAR_DURATION = 0.04f;

        private Tween textProgressTween;
        private TextWriter textWriter;

        public Dialogue CurrentDialogue { get; private set; }

        public void DialogueSetup(Dialogue dialogue)
        {
            CurrentDialogue = dialogue;
            if (CurrentDialogue != null)
            {
                dialogueSprite.sprite = CurrentDialogue.DisplayInfo.Sprite;
                dialogueTitle.text = CurrentDialogue.DisplayInfo.Title;

                dialogueSprite.enabled = dialogueSprite.sprite != null;

                textWriter = new TextWriter(CurrentDialogue.LocalizedText);
            }
        }

        public void PanelShow()
        {
            dialoguePanel.SetActive(true);
        }

        public void DialogueStart()
        {
            ActionsManager.Instance.OnDialogueStart?.Invoke();
            StartCoroutine(DialogueDisplay());
        }

        private static readonly WaitForSeconds displayDelay = new(0.2f);
        private IEnumerator DialogueDisplay()
        {
            DisplayStart();

            while (textWriter.Progress < 1.0f)
            {
                DisplayUpdate();
                yield return null;
            }

            skipDialogueButton.gameObject.SetActive(!(CurrentDialogue is DialogueWithChoice));

            if (CurrentDialogue is DialogueWithChoice)
            {
                DialogueWithChoice current = CurrentDialogue as DialogueWithChoice;
                current.Select(0);
                choicesPanel.Setup(current.Choices);
                yield return choicesPanel.Display();
            }

            yield return displayDelay;

            DisplayFinish();
        }

        private void DisplayStart()
        {
            PanelShow();
            textProgressTween = DOTween.To(
                getter: () => textWriter.Progress,
                setter: (value) => textWriter.Progress = value,
                endValue: 1.0f,
                duration: textWriter.TotalLength * CHAR_DURATION
            ).SetEase(Ease.Linear);
        }

        private void DisplayUpdate()
        {
            dialogueText.text = textWriter.CurrentText;
        }

        private void DisplayFinish()
        {
            textProgressTween.Kill(true);
            textProgressTween = null;
            textWriter = null;
            dialogueText.text = CurrentDialogue.LocalizedText;
            choicesPanel.Hide();
        }
        private bool IsDisplayFinished => textWriter == null;

        private void DialogueNext()
        {
            Dialogue nextDialogue = CurrentDialogue.Next;
            if (nextDialogue != null)
            { 
                DialogueSetup(nextDialogue);
                DialogueStart();
            }
            else
            { DialogueFinish(); }
        }

        public void DialogueCancel()
        {
            DisplayFinish();
            CurrentDialogue = null;
            DialogueFinish();
        }

        private void DialogueFinish()
        {
            ActionsManager.Instance.OnDialogueEnd?.Invoke();
            PanelHide();

            //TO DO - REMOVER DEPOIS
            /*if (CurrentDialogue.DisplayInfo is CharacterDisplayInfo)
            {
                var character = (CurrentDialogue.DisplayInfo as CharacterDisplayInfo).Character;
                ActionsManager.Instance.OnCharacterMinigameSelected?.Invoke(character);
            }*/
        }

        private void PanelHide()
        {
            dialoguePanel.SetActive(false);
        }
    }
}
