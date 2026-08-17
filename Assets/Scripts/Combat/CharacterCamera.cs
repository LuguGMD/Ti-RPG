using Unity.Cinemachine;
using UnityEngine;

namespace RPG
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] CinemachineCamera _camera;

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterSelected += ChangeCameraTarget;
            ActionsManager.Instance.OnActionStart += EnableCamera;
            ActionsManager.Instance.OnActionEnd += DisableCamera;
            ActionsManager.Instance.OnCharacterDeselected += DisableCamera;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterSelected -= ChangeCameraTarget;
            ActionsManager.Instance.OnActionStart -= EnableCamera;
            ActionsManager.Instance.OnActionEnd -= DisableCamera;
            ActionsManager.Instance.OnCharacterDeselected -= DisableCamera;
        }

        private void ChangeCameraTarget(Combat.CharacterController character)
        {
            _camera.Target.TrackingTarget = character.transform;
        }

        private void EnableCamera()
        {
            if (_camera.Target.TrackingTarget != null)
            {
                _camera.enabled = true;
            }
        }

        private void DisableCamera()
        {
            _camera.Target.TrackingTarget = null;
            _camera.enabled = false;
        }

    }
}
