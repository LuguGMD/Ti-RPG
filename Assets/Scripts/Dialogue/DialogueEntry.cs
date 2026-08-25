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
        private bool _isTalking = false;


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
            //TO DO adicioanr verificacao de vitoria ou derrota de uma fase
            _graphPicked = _graph; 
            //_graphPicked = _graphDemotivated; 
        }

        public void StartDialogue()
        {
            if (!DialogueController.Instance.IsRunning)
            {
                _isTalking = true;
                DialogueController.Instance.DialogueSetup(_graphPicked.Entry.Dialogue);
                DialogueController.Instance.DialogueStart();
                CameraManager.Instance.SwitchCamera(_camera);
            }
        }

        private void OnDialogueEnd()
        {
            if (_isTalking)
            {
                CameraManager.Instance.DisableCamera(_camera);
                _isTalking = false;
            }
        }
    }
}
