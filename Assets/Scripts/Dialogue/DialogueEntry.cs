using RPG.Camera;
using RPG.Dialogue;
using RPG.Combat;
using RPG.Management.Interaction;
using Unity.Cinemachine;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueEntry : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGraphRuntime _graph;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private CharacterScriptable _characterInfo;

        private void OnEnable()
        {
            ActionsManager.Instance.OnDialogueEnd += OnDialogueEnd;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnDialogueEnd -= OnDialogueEnd;
        }

        public void Interact()
        {
            StartDialogue();
        }

        public void StartDialogue()
        {
            if (!DialogueController.Instance.IsRunning)
            {
                DialogueController.Instance.DialogueSetup(_graph.Entry.Dialogue);
                DialogueController.Instance.DialogueStart();
                CameraManager.Instance.SwitchCamera(_camera);
            }
        }

        private void OnDialogueEnd()
        {
            CameraManager.Instance.DisableCamera(_camera);
            if(_characterInfo != null)
            {
                ActionsManager.Instance.OnCharacterMinigameSelected?.Invoke(_characterInfo);
            }
        }
    }
}
