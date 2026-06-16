using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueChoiceController : MonoBehaviour
    {
        private IReadOnlyList<Dialogue> choices;
        public void Setup(IReadOnlyList<Dialogue> choices)
        {
            this.choices = choices;
        }

        private static readonly WaitForSeconds choiceDelay = new(0.1f);
        public IEnumerator Display()
        {
            gameObject.SetActive(true);
            foreach (Dialogue choice in choices)
            {
                // TODO: mostrar botão de cada escolha
                yield return choiceDelay;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
