using Unity.Cinemachine;
using UnityEngine;

namespace RPG.Management.Minigames
{
    public class MinigameCamera : MonoBehaviour
    {
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
        }
    }
}
