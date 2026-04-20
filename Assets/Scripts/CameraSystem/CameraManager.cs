using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        private List<BaseCamera> cameras = new List<BaseCamera>();
        private BaseCamera currentCamera;
        private BaseCamera previousCamera;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("CameraManager já existe. Destruindo duplicata.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializeCameras();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void InitializeCameras()
        {
            cameras.Clear();

            var camerasFound = FindObjectsByType<BaseCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var camera in camerasFound)
            {
                if (camera != null)
                {
                    cameras.Add(camera);
                    camera.Deactivate();
                }
            }

            Debug.Log($"[CameraManager] {cameras.Count} câmeras registradas");
        }

        public void SwitchCamera(BaseCamera newCamera)
        {
            if (newCamera == null)
            {
                Debug.LogError("[CameraManager] Câmera não pode ser nula.");
                return;
            }

            if (!cameras.Contains(newCamera))
            {
                Debug.LogError("[CameraManager] Câmera não está registrada no CameraManager");
                return;
            }

            if (currentCamera == newCamera)
            {
                Debug.LogWarning("[CameraManager] Câmera já está ativa");
                return;
            }

            previousCamera = currentCamera;
            currentCamera?.Deactivate();
            currentCamera = newCamera;
            currentCamera.Activate();
        }

        public void SwitchCameraByIndex(int index)
        {
            if (index < 0 || index >= cameras.Count)
            {
                Debug.LogError($"[CameraManager] Índice inválido: {index}. Total de câmeras: {cameras.Count}");
                return;
            }

            SwitchCamera(cameras[index]);
        }

        public void SwitchCameraByName(string cameraName)
        {
            if (string.IsNullOrEmpty(cameraName))
            {
                Debug.LogError("[CameraManager] Nome da câmera não pode ser nulo ou vazio");
                return;
            }

            var camera = cameras.Find(c => c.gameObject.name == cameraName);

            if (camera == null)
            {
                Debug.LogError($"[CameraManager] Câmera com nome '{cameraName}' não encontrada");
                return;
            }

            SwitchCamera(camera);
        }

        public void SetCameraTarget(BaseCamera targetCamera, Transform newTarget)
        {
            if (targetCamera == null)
            {
                Debug.LogError("[CameraManager] Câmera não pode ser nula.");
                return;
            }

            if (!cameras.Contains(targetCamera))
            {
                Debug.LogError("[CameraManager] Câmera não está registrada no CameraManager");
                return;
            }

            targetCamera.SetTarget(newTarget);
        }

        public void SetCurrentCameraTarget(Transform newTarget)
        {
            if (currentCamera == null)
            {
                Debug.LogError("[CameraManager] Nenhuma câmera ativa no momento");
                return;
            }

            SetCameraTarget(currentCamera, newTarget);
        }

        public void ReturnToPreviousCamera()
        {
            if (previousCamera == null)
            {
                Debug.LogWarning("[CameraManager] Nenhuma câmera anterior disponível");
                return;
            }

            SwitchCamera(previousCamera);
        }

        public BaseCamera GetCurrentCamera() => currentCamera;

        public List<BaseCamera> GetAllCameras() => cameras;

        public BaseCamera GetCameraByName(string cameraName)
        {
            if (string.IsNullOrEmpty(cameraName))
            {
                Debug.LogWarning("[CameraManager] Nome da câmera não pode ser nulo ou vazio");
                return null;
            }

            return cameras.Find(c => c.gameObject.name == cameraName);
        }

        public int GetCameraCount() => cameras.Count;
    }
}