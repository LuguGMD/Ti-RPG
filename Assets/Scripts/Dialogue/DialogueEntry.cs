using RPG.Camera;
using RPG.Dialogue;
using RPG.Combat;
using RPG.Management.Interaction;
using Unity.Cinemachine;
using UnityEngine;
using RPG.Save;

namespace RPG.Dialogue
{
    public class DialogueEntry : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGraphRuntime _graph;
        [SerializeField] private DialogueGraphRuntime _graphDemotivated;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private CharacterScriptable _characterInfo;

        private DialogueGraphRuntime _graphPicked;


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
            PickDialogue();
            StartDialogue();
        }

        private void PickDialogue()
        {
            if (!GameManager.DefeatedCharacters.Contains(_characterInfo))
            { _graphPicked = _graph; }
            else
            { _graphPicked = _graphDemotivated; }
        }

        public void StartDialogue()
        {
            if (!DialogueController.Instance.IsRunning)
            {
                DialogueController.Instance.DialogueSetup(_graphPicked.Entry.Dialogue);
                DialogueController.Instance.DialogueStart();
                CameraManager.Instance.SwitchCamera(_camera);
            }
        }

        private void OnDialogueEnd()
        {
            CameraManager.Instance.DisableCamera(_camera);
            if(_characterInfo != null && GameManager.DefeatedCharacters.Contains(_characterInfo))
            {
                ActionsManager.Instance.OnCharacterMinigameSelected?.Invoke(_characterInfo);
            }
        }
    }
}
