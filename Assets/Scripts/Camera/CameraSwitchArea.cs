using UnityEngine;

namespace RPG.Camera
{
    public class CameraSwitchArea : MonoBehaviour
    {
        [SerializeField] private GameObject _camera;

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
