using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using Lugu.Singleton;
using RPG.Combat;
using RPG.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace RPG
{
    public class DialogueChoiceController : MonoBehaviour
    {
        private IReadOnlyList<NewDialogue> choices;
        public void Setup(IReadOnlyList<NewDialogue> choices)
        {
            this.choices = choices;
        }

        private static readonly WaitForSeconds choiceDelay = new(0.1f);
        public IEnumerator Display()
        {
            gameObject.SetActive(true);
            foreach (NewDialogue choice in choices)
            {
                // TODO: adicionar botão de cada escolha
                yield return choiceDelay;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

    public class DialogueController : SingletonMono<DialogueController>
    {
        private PlayerInput player;
        
        [Header("UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private DialogueChoiceController choicesPanel;
        [SerializeField] private Image dialogueSprite;
        [SerializeField] private TextMeshProUGUI dialogueTitle;
        [SerializeField] private TextMeshProUGUI dialogueText;

        protected new void Awake()
        {
            base.Awake();
            player = FindAnyObjectByType<PlayerInput>();
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

        private void HandleInput()
        {
            if (textWriter.Progress < 1.0f)
            { textWriter.Progress = 1.0f; }
            else if (IsDisplayFinished)
            { DialogueNext(); }
        }

        protected void OnEnable()
        { EnableInput(); }

        protected void OnDisable()
        { DisableInput(); }

        private const float CHAR_DURATION = 0.04f;

        private Tween textProgressTween;
        private TextWriter textWriter;

        public NewDialogue CurrentDialogue { get; private set; }

        public void DialogueSetup(NewDialogue dialogue)
        {
            CurrentDialogue = dialogue;
            if (CurrentDialogue != null)
            {
                dialogueSprite.sprite = CurrentDialogue.DisplayInfo.Sprite;
                dialogueTitle.text = CurrentDialogue.DisplayInfo.Title;

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
            
            if (CurrentDialogue is NewDialogue.WithChoice)
            {
                NewDialogue.WithChoice current = CurrentDialogue as NewDialogue.WithChoice;

                choicesPanel.Setup(current.Choices);
                yield return choicesPanel.Display();
            }
            
            yield return displayDelay;
            
            DisplayFinish();
        }

        private void DisplayStart()
        {
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
        }
        private bool IsDisplayFinished => textWriter == null;

        private void DialogueNext()
        {
            NewDialogue nextDialogue = CurrentDialogue.Next;
            if (nextDialogue != null)
            { DialogueSetup(nextDialogue); }
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
            if (CurrentDialogue.DisplayInfo is CharacterDisplayInfo)
            {
                var character = (CurrentDialogue.DisplayInfo as CharacterDisplayInfo).Character;
                ActionsManager.Instance.OnCharacterMinigameSelected?.Invoke(character);
            }
        }

        private void PanelHide()
        {
            dialoguePanel.SetActive(false);
        }
    }

    public class DialogueDisplayInfo
    {
        readonly public string Title;
        readonly public Sprite Sprite;

        public DialogueDisplayInfo(string title, Sprite sprite)
        {
            Title = title;
            Sprite = sprite;
        }
    }

    public class CharacterDisplayInfo : DialogueDisplayInfo
    {
        readonly public CharacterScriptable Character;
        public CharacterDisplayInfo(CharacterScriptable character)
        : base(character.EntityName, character.Icon)
        {
            Character = character;
        }
    }

    public class NewDialogue
    {
        static private StringTable localizationTable;
        static public LocalizedStringTable LocalizationTable
        { set => localizationTable = value.GetTable(); }

        readonly public string Key;
        readonly public DialogueDisplayInfo DisplayInfo;

        public NewDialogue(string key, DialogueDisplayInfo displayInfo, NewDialogue next)
        {
            Key = key;
            DisplayInfo = displayInfo;

            Next = next;
        }

        public NewDialogue Next { get; private set; }

        public string LocalizedText => localizationTable[Key].LocalizedValue;

        public class WithChoice : NewDialogue
        {
            private NewDialogue[] choices;
            public IReadOnlyList<NewDialogue> Choices => choices;

            public WithChoice(string key, DialogueDisplayInfo displayInfo, NewDialogue[] choices)
            : base(key, displayInfo, null)
            {
                this.choices = choices;
            }

            public void Select(int index)
            {
                NewDialogue entry = choices[index];
                Next = entry;
            }
        }
    }


    public class TextWriter
    {
        private readonly char[] textChars;
        private readonly List<(int, string)> tags;

        public TextWriter(string text)
        {
            textChars = new char[text.Length];
            tags = new();

            StringBuilder tagBuffer = null;
            int parsedIndex = 0;
            for (int textIndex = 0; textIndex < text.Length; textIndex++)
            {
                char currentChar = text[textIndex];
                if (currentChar == '<')
                {
                    tagBuffer = new();
                }

                if (tagBuffer != null)
                {
                    tagBuffer.Append(currentChar);

                    if (currentChar == '>')
                    {
                        tags.Add((parsedIndex, tagBuffer.ToString()));
                        tagBuffer = null;
                        parsedIndex++;
                    }
                }
                else
                {
                    textChars[parsedIndex] = currentChar;
                    parsedIndex++;
                }
            }
            if (tagBuffer != null)
            {
                tagBuffer.CopyTo(0, textChars, parsedIndex, tagBuffer.Length);
                parsedIndex += tagBuffer.Length;
            }

            if (parsedIndex < textChars.Length)
            { Array.Resize(ref textChars, parsedIndex); }

            Progress = 0.0f;
        }

        public float TotalLength => textChars.Length;
        public int CurrentLength { get; set; }
        public float Progress
        {
            get => CurrentLength / textChars.Length;
            set => CurrentLength = Mathf.FloorToInt(Mathf.Clamp01(value) * textChars.Length);
        }
        public string CurrentText => ToString();

        public override string ToString()
        {
            StringBuilder textBuilder = new();
            textBuilder.Append(textChars);

            textBuilder.Insert(CurrentLength, "<color=#0000>");
            textBuilder.Append("</color>");

            for (int tagIndex = tags.Count - 1; 0 <= tagIndex; --tagIndex)
            {
                (int tagTextIndex, string tag) = tags[tagIndex];

                if (tagTextIndex <= CurrentLength)
                { textBuilder.Insert(tagTextIndex, tag); }
                else
                { break; }
            }

            return textBuilder.ToString();
        }
    }
}
