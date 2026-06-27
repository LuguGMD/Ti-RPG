using RPG.Dialogue;
using RPG.Management.Interaction;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueEntry : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGraphRuntime graph;

        public void Interact()
        {
            StartDialogue();
        }

        public void StartDialogue()
        {
            if (!DialogueController.Instance.IsRunning)
            {
                DialogueController.Instance.DialogueSetup(graph.Entry.Dialogue);
                DialogueController.Instance.DialogueStart();
            }
        }
    }
}
