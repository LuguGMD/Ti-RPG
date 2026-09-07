using UnityEngine;
using RPG.Input;

namespace RPG.Management.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Configurações de Movimento")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float gravity;

        [Header("Referências")]
        private CharacterController controller;
        private PlayerInput playerInput;
        [SerializeField] private Transform modelTransform;

        [Header("Movimento por Clique")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float clickStopDistance = 0.1f;

        private Vector2 inputDirection;
        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;
        private Vector3 clickTarget;

        private float currentSpeed;
        private bool hasClickTarget;

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
            if (ManagementManager.IsInteractionRunning)
                input2d = Vector2.zero;

            inputDirection = input2d;

            currentSpeed = Mathf.Clamp01(inputDirection.magnitude) * maxSpeed;
        }

        private void Update()
        {
            HandleMouseClick();

            CalculateMoveDirection();
            ApplyMovement();
            ApplyRotation();
        }

        private void HandleMouseClick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
                {
                    clickTarget = hit.point;
                    hasClickTarget = true;
                }
            }
        }

        private void CalculateMoveDirection()
        {
            if (inputDirection.sqrMagnitude > 0.01f)
            {
                moveDirection =
                    (UnityEngine.Camera.main.transform.right * inputDirection.x) +
                    (UnityEngine.Camera.main.transform.forward * inputDirection.y);

                moveDirection.y = 0;
                moveDirection = moveDirection.normalized;

                hasClickTarget = false;
            }
            else if (hasClickTarget)
            {
                Vector3 directionToTarget = clickTarget - transform.position;
                directionToTarget.y = 0;

                if (directionToTarget.magnitude <= clickStopDistance)
                {
                    moveDirection = Vector3.zero;
                    hasClickTarget = false;
                    currentSpeed = 0;
                }
                else
                {
                    moveDirection = directionToTarget.normalized;
                    currentSpeed = maxSpeed;
                }
            }
            else
            {
                moveDirection = Vector3.zero;
                currentSpeed = 0;
            }

            if (moveDirection != Vector3.zero)
            {
                lastMoveDirection = moveDirection;
            }
        }

        private void ApplyMovement()
        {
            controller.Move(
                moveDirection * (currentSpeed * Time.deltaTime) +
                (Vector3.down * gravity * Time.deltaTime)
            );
        }

        private void ApplyRotation()
        {
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                modelTransform.rotation = Quaternion.Slerp(
                    modelTransform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}