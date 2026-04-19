using UnityEngine;

namespace RPG.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool _lockX;
        [SerializeField] private bool _lockY;
        [SerializeField] private bool _lockZ;

        [SerializeField] private bool _invertX;
        [SerializeField] private bool _invertY;
        [SerializeField] private bool _invertZ;

        private const float PERSPECTIVE_DISTANCE = 20f;

        private void LateUpdate()
        {
            Vector3 previousRotation = transform.localEulerAngles;

            Vector3 lookAtPosition = Camera.main.transform.position;
            lookAtPosition += -Camera.main.transform.forward * PERSPECTIVE_DISTANCE;

            transform.LookAt(lookAtPosition);
            Vector3 newRotation = transform.localEulerAngles;

            if(_invertX) transform.localEulerAngles = new Vector3(-newRotation.x, newRotation.y, newRotation.z);
            newRotation = transform.localEulerAngles;
            if (_invertY) transform.localEulerAngles = new Vector3(newRotation.x, -newRotation.y, newRotation.z);
            newRotation = transform.localEulerAngles;
            if (_invertZ) transform.localEulerAngles = new Vector3(newRotation.x, newRotation.y, -newRotation.z);
            newRotation = transform.localEulerAngles;

            if (_lockX) transform.localEulerAngles = new Vector3(previousRotation.x, newRotation.y, newRotation.z);
            newRotation = transform.localEulerAngles;
            if (_lockY) transform.localEulerAngles = new Vector3(newRotation.x, previousRotation.y, newRotation.z);
            newRotation = transform.localEulerAngles;
            if (_lockZ) transform.localEulerAngles = new Vector3(newRotation.x, newRotation.y, previousRotation.z);

        }
    }
}
