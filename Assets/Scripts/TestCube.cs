using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CursorTarget))]
    public class TestCube : MonoBehaviour
    {
        [SerializeField, Tooltip("Speed, in meters per second")]
        private readonly float speed = 5f;

        private Vector3 velocity;

        private PlayerInput input;
        private CursorTarget cursor;

        protected void Awake()
        {
            input = GetComponent<PlayerInput>();
            cursor = GetComponent<CursorTarget>();
        }

        protected void Start()
        {
            input.Actions.Move.OnUpdate((input) =>
                SetVelocity(new(input.x, 0, input.y), speed)
            );

            cursor.Actions.Hover.OnStart(() =>
                Debug.Log("Hovered!")
            );

            cursor.Actions.LeftClick.OnStart(() =>
                Debug.Log("Left Click!")
            );

            cursor.Actions.RightClick.OnStart(() =>
                Debug.Log("Right Click!")
            );
            
        }

        private void Update()
        {
            UpdatePosition(Time.deltaTime);
        }

        private void UpdatePosition(float deltaTime)
        {
            transform.position += velocity * deltaTime;
        }

        private void SetVelocity(Vector3 direction, float speed)
        {
            velocity = direction * speed;
        }
    }
}
