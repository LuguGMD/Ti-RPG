using RPG.Input;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Management.Interaction
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteract : MonoBehaviour
    {
        private PlayerInput _playerInput;
        [Tooltip("O raio da área ao redor do apresentador onde a interação é possível.")]
        [SerializeField] private float interactionRadius = 3f;
        [SerializeField] private GameObject _interactPreviewCanvas;
        [SerializeField] private Button _interactButton;
        [SerializeField] private Vector3 _previewCanvasOffset;
        private IInteractable _closestInteractable;
        private GameObject _closestInteractableGO;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _interactButton.onClick.AddListener(InteractWithClosest);
        }

        private void Start()
        {
            _playerInput.Actions.Interact.OnStart(InteractWithClosest);
        }

        private void Update()
        {
            GetClosestInteractable();

            if (_closestInteractable != null)
            {
                _interactPreviewCanvas.transform.position = _closestInteractableGO.transform.position + _previewCanvasOffset;
                _interactPreviewCanvas.SetActive(true);
            }
            else
            {
                _interactPreviewCanvas.SetActive(false);
            }
        }

        private void InteractWithClosest()
        {
            if (ManagementManager.IsInteractionRunning) return;

            GetClosestInteractable();

            if (_closestInteractable != null)
            {
                _closestInteractable.Interact();
            }
        }

        private void GetClosestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);

            _closestInteractable = null;
            float closestDistance = float.MaxValue;

            foreach (Collider collider in colliders)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    float distanceToInteractable = Vector3.Distance(transform.position, collider.transform.position);

                    if (distanceToInteractable < closestDistance)
                    {
                        closestDistance = distanceToInteractable;
                        _closestInteractable = interactable;
                        _closestInteractableGO = collider.gameObject;

                        _previewCanvasOffset.y = collider.bounds.extents.y * 2.2f;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}