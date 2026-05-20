using Unity.Cinemachine;
using UnityEngine;

namespace RPG.Camera
{
    public class CameraSwitchArea : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;

        private void OnTriggerEnter(Collider other)
        {
            CameraManager.Instance.SwitchCamera(_camera);
        }

        private void OnTriggerExit(Collider other)
        {
            CameraManager.Instance.DisableCamera(_camera);
        }
    }
}
