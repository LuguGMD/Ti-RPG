using UnityEngine;

namespace RPG.Combat.Cameras
{
    [DefaultExecutionOrder(-50)]
    public class CombatCameraFocusTarget : MonoBehaviour
    {
        [SerializeField] private Transform _fallbackTarget;
        [SerializeField] private Vector3 _localOffset = new Vector3(0f, 1.2f, 0f);
        [SerializeField] private float _positionSmoothTime = 0.18f;
        [SerializeField] private float _rotationSmoothTime = 0.25f;
        [Tooltip("Copia o yaw do alvo. Use apenas no proxy da camera de inimigo.")]
        [SerializeField] private bool _copyTargetYaw = false;

        private Transform _transform;
        private Transform _target;
        private Vector3 _velocity;
        private float _yaw;
        private float _yawVelocity;

        public Transform Target { get { return _target; } }

        private Transform ActiveTarget
        {
            get { return _target != null ? _target : _fallbackTarget; }
        }
        private void Awake()
        {
            _transform = transform;
            _yaw = _transform.eulerAngles.y;
        }
        private void Start()
        {
            Snap();
        }
        public void SetTarget(Transform target, bool snap = false)
        {
            _target = target;

            if (snap) Snap();
        }
        private void Snap()
        {
            Transform target = ActiveTarget;

            _transform.position = DesiredPosition(target);
            _velocity = Vector3.zero;
            _yawVelocity = 0f;

            if (!_copyTargetYaw) return;

            _yaw = DesiredYaw(target);
            _transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        }
        private void LateUpdate()
        {
            Transform target = ActiveTarget;

            if (target == null) return;

            _transform.position = Vector3.SmoothDamp(
                _transform.position,
                DesiredPosition(target),
                ref _velocity,
                _positionSmoothTime
            );

            if (!_copyTargetYaw) return;

            _yaw = Mathf.SmoothDampAngle(_yaw, DesiredYaw(target), ref _yawVelocity, _rotationSmoothTime);
            _transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        }
        private Vector3 DesiredPosition(Transform target)
        {
            if (target == null) return _localOffset;

            return _copyTargetYaw
                ? target.position + (target.rotation * _localOffset)
                : target.position + _localOffset;
        }
        private float DesiredYaw(Transform target)
        {
            return target != null ? target.eulerAngles.y : _yaw;
        }
    }
}
