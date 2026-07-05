using Unity.Cinemachine;
using UnityEngine;

namespace RPG
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] CinemachineCamera _camera;

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterSelected += EnableCamera;
            ActionsManager.Instance.OnCharacterDeselected += DisableCamera;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterSelected -= EnableCamera;
            ActionsManager.Instance.OnCharacterDeselected -= DisableCamera;
        }

        private void EnableCamera(Combat.CharacterController character)
        {
            _camera.Target.TrackingTarget = character.transform;
            _camera.enabled = true;
        }

        private void DisableCamera()
        {
            _camera.Target.TrackingTarget = null;
            _camera.enabled = false;
        }

    }
}
