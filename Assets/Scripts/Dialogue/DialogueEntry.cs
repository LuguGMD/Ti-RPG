using RPG.Dialogue;
using UnityEngine;

namespace RPG
{
    public class DialogueEntry : MonoBehaviour
    {
        [SerializeField] private DialogueEntrySO entry;

        public void StartDialogue()
        {
            DialogueController.Instance.DialogueSetup(entry.Dialogue);
            DialogueController.Instance.DialogueStart();
        }
    }
}
