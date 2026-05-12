using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configurações de Interação")]
    [Tooltip("O raio da área ao redor do apresentador onde a interação é possível.")]
    public float interactionRadius = 3f;
    
    [Tooltip("A tecla usada para interagir.")]
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            InteractWithClosest();
        }
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