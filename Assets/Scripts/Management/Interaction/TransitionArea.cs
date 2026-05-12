using UnityEngine;

namespace RPG.Management.Interaction
{
    public class TransitionArea : MonoBehaviour, IInteractable
    {
        [SerializeField] private ScenesEnum _scene;

        public void Interact()
        {
            GameManager.ChangeScene(_scene);
        }
    }
}
