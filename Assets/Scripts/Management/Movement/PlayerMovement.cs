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
        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform modelTransform;

        // Variáveis internas
        private Vector2 inputDirection;
        private float currentSpeed;
        public float VelocidadeAtual => currentSpeed;
        public float VelocidadeMaxima => maxSpeed;
        public Vector2 Direcao => inputDirection;

        private void Awake()
        {
            if (controller == null)
            {
                controller = GetComponent<CharacterController>();
            }
            if (playerInput == null)
            {
                playerInput = GetComponent<PlayerInput>();
            }
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
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            Vector3 moveDirection = (transform.right * inputDirection.x) + (transform.forward * inputDirection.y);
            moveDirection = moveDirection.normalized;
            controller.Move(moveDirection * (currentSpeed * Time.deltaTime));

            ApplyRotation(moveDirection);
        }

        private void ApplyRotation(Vector3 moveDirection)
        {
            if (moveDirection.sqrMagnitude > 0.01f && modelTransform != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
