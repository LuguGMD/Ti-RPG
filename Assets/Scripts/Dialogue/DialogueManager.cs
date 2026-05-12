using RPG.Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace RPG
{
    public class DialogueManager : MonoBehaviour
    {
        private PlayerInput player;

        [SerializeField] private LocalizedStringTable dialogueTable;
        [SerializeField] private string entryDialogueKey = "LINE_1";

        private StringTable table;

        protected void Awake()
        {
            if (entryDialogueKey == null || entryDialogueKey == string.Empty)
            {
                Debug.LogWarning(
                    $"{gameObject.name} > {nameof(Dialogue)}: " +
                    $"Nenhum diálogo de entrada foi definido."
                );
            }

            player = GetComponent<PlayerInput>();
            table = dialogueTable.GetTable();
        }

        public Dialogue Current { get; private set; }

        public void Next()
        {
            Current = Current == null ?
                Dialogue.Get(table, entryDialogueKey)
              : Current.GetNext();
        }

        #region Examples

        [SerializeField] private TMPro.TMP_Text textField;
        [SerializeField, Range(0, 1)] private float lineProgress;
        private TextProgress textProgress;

        protected void Start()
        {
            player.Actions.Jump.OnStart(() =>
            {
                Next();
                Display();
                if (Current != null)
                {
                    textProgress = new TextProgress(Current.TextLocalized);
                }
            });
        }

        protected void Update()
        {
            if (Current != null)
            {
                textProgress.SetProgress(lineProgress);
                textField.text = textProgress.ToString();
            }
        }

        public void Display()
        {
            switch (Current)
            {
                case Dialogue.WithChoice:
                    Dialogue.WithChoice current = Current as Dialogue.WithChoice;
                    Debug.Log(
                        Current.Key + '\n' +
                        Current.TextLocalized
                    );
                    foreach (Dialogue.Choice choice in current.Choices)
                    {
                        Debug.Log(
                            choice.Key + '\n' +
                            choice.TextLocalized
                        );
                    }
                    break;

                case Dialogue:
                    Debug.Log(
                        Current.Key + '\n' +
                        Current.TextLocalized
                    );
                    break;

                default:
                    Debug.Log("Fim do Diálogo.");
                    break;
            }
        }

        #endregion
    }
}
