using UnityEngine;

namespace RPG.Management.Movement
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerAnimation : MonoBehaviour
    {

        private Animator[] _animators;
        private PlayerMovement _movement;

        private void Awake()
        {
            _animators = GetComponentsInChildren<Animator>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            foreach (Animator animator in _animators)
            {
                animator.SetFloat("Speed", _movement.CurrentSpeed);
            }
        }

    }
}
