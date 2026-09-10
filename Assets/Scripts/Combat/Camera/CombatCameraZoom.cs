using LucasRozado.Utility;
using RPG.Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Combat.Cameras
{
    [RequireComponent(typeof(CinemachineOrbitalFollow))]
    public class CombatCameraZoom : MonoBehaviour
    {
        [SerializeField] private float _stepPerNotch = 0.12f;
        [SerializeField] private float _smoothTime = 0.12f;
        [SerializeField] private bool _blockWhenCursorOverUI = true;

        private CinemachineOrbitalFollow _orbital;
        private float _targetRadial;
        private float _velocity;
        private bool _isPaused;

        private void Awake()
        {
            _orbital = GetComponent<CinemachineOrbitalFollow>();
            _targetRadial = _orbital.RadialAxis.Value;
        }

        private void OnEnable()
        {
            CursorInput.Actions.ScrollWheel.OnUpdate(HandleScroll);
            ActionsManager.Instance.OnPauseToggle += HandlePauseToggle;
        }

        private void OnDisable()
        {
            CursorInput.Actions.ScrollWheel.Remove.OnUpdate(HandleScroll);
            ActionsManager.Instance.OnPauseToggle -= HandlePauseToggle;
        }

        private void HandleScroll(Vector2 scroll)
        {
            if (_isPaused) return;

            if (Mathf.Approximately(scroll.y, 0f)) return;

            if (_blockWhenCursorOverUI && IsCursorOverUI()) return;

            Vector2 range = _orbital.RadialAxis.Range;

            _targetRadial = Mathf.Clamp(
                _targetRadial - (Mathf.Sign(scroll.y) * _stepPerNotch),
                range.x,
                range.y
            );
        }

        private bool IsCursorOverUI()
        {
            if (EventSystem.current == null) return false;

            return Utility.Get(EventSystem.current)
                .IsCursorOverUIElement(CursorInput.Actions.Position.LastValue);
        }

        private void HandlePauseToggle(bool isPaused)
        {
            _isPaused = isPaused;
        }

        private void LateUpdate()
        {
            float value = _orbital.RadialAxis.Value;

            if (Mathf.Approximately(value, _targetRadial)) return;

            _orbital.RadialAxis.Value = Mathf.SmoothDamp(value, _targetRadial, ref _velocity, _smoothTime);
        }
    }
}
