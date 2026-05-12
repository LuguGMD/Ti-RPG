using UnityEngine;
using RPG.Input;

namespace RPG.Management.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Configurações de Movimento")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Referências")]
        private CharacterController controller;
        private PlayerInput playerInput;
        [SerializeField] private Transform modelTransform;

        private Vector2 inputDirection;
        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;
        private float currentSpeed;

        #region Properties

        public float CurrentSpeed => currentSpeed;
        public float MaxSpeed => maxSpeed;
        public Vector3 LastMoveDirection => lastMoveDirection;

        #endregion

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            playerInput.Actions.Move.OnUpdate((input2d) => MovePlayer(input2d));
        }

        private void MovePlayer(Vector2 input2d)
        {
            inputDirection = input2d;
            currentSpeed = Mathf.Clamp01(inputDirection.magnitude) * maxSpeed;
        }

        private void Update()
        {
            CalculateMoveDirection();
            ApplyMovement();
            ApplyRotation();
        }

        private void CalculateMoveDirection()
        {
            moveDirection = (UnityEngine.Camera.main.transform.right * inputDirection.x) + (UnityEngine.Camera.main.transform.forward * inputDirection.y);
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;

            if (moveDirection != Vector3.zero)
            {
                lastMoveDirection = moveDirection;
            }
        }

        private void ApplyMovement()
        {
            controller.Move(moveDirection * (currentSpeed * Time.deltaTime));
        }

        private void ApplyRotation()
        {
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
