using RPG.Input;
using UnityEngine;

namespace RPG.Management.Interaction
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInteract : MonoBehaviour
    {
        private PlayerInput _playerInput;
        [Tooltip("O raio da área ao redor do apresentador onde a interação é possível.")]
        [SerializeField] private float interactionRadius = 3f;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            _playerInput.Actions.Interact.OnStart(InteractWithClosest);
        }

        private void InteractWithClosest()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

            IInteractable closestInteractable = null;
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
                        closestInteractable = interactable;
                    }
                }
            }

            if (closestInteractable != null)
            {
                closestInteractable.Interact();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}