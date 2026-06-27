using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueChoiceController : MonoBehaviour
    {
        private IReadOnlyList<Dialogue> choices;
        [SerializeField] private DialogueOptionButton _dialogueOptionButtonPrefab;
        private List<DialogueOptionButton> _options = new List<DialogueOptionButton>();

        public void Setup(IReadOnlyList<Dialogue> choices)
        {
            this.choices = choices;
        }

        private static readonly WaitForSeconds choiceDelay = new(0.1f);
        public IEnumerator Display()
        {
            _options = new List<DialogueOptionButton>();

            gameObject.SetActive(true);
            foreach (Dialogue choice in choices)
            {
                DialogueOptionButton optionButton = Instantiate<DialogueOptionButton>(_dialogueOptionButtonPrefab, transform);
                optionButton.Init(choice, _options.Count);
                _options.Add(optionButton);
                yield return choiceDelay;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            foreach(DialogueOptionButton optionButton in _options)
            {
                Destroy(optionButton.gameObject);
            }

            _options.Clear();
        }
    }
}
