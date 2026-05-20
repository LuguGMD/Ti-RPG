using Lugu.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace RPG.Camera
{
    public class CameraManager : SingletonMono<CameraManager>
    {
        [SerializeField]
        private List<CinemachineCamera> cameras;

        private CinemachineCamera currentCamera;
        private CinemachineCamera previousCamera;

        [SerializeField]
        [Tooltip("Selecione a câmera pelo índice no inspetor")]
        private int cameraNumber = 0;

        #region Properties

        public CinemachineCamera CurrentCamera { get { return currentCamera; } }
        public CinemachineCamera PreviousCamera { get { return previousCamera; } }
        public int CameraNumber { get { return cameraNumber; } }

        #endregion

        #region Unity Methods

        private void Start()
        {
            InitializeCameras();
        }

        private void Update()
        {
            ValidateCameraSwitch();
        }

        #endregion

        #region Methods
        private void InitializeCameras()
        {
            if (cameras.Count == 0)
            {
                Debug.LogWarning("Nenhuma câmera encontrada!");
                return;
            }

            foreach (CinemachineCamera cam in cameras)
            {
                cam.gameObject.SetActive(false);
            }

            SwitchCamera(cameras[cameraNumber]);
        }

        //apenas para teste de trocar a camera no inspctor
        void ValidateCameraSwitch()
        {

            if (cameras.Count > 0)
            {
                cameraNumber = Mathf.Clamp(cameraNumber, 0, cameras.Count - 1);

                if (currentCamera != cameras[cameraNumber])
                {
                    SwitchCamera(cameras[cameraNumber]);
                }
            }
        }

        public void SwitchCamera(CinemachineCamera newCamera)
        {
            if (newCamera == null)
            {
                Debug.LogWarning("Não tem camera!");
                return;
            }

            if (currentCamera == newCamera)
                return;

            if (currentCamera != null)
            {
                previousCamera = currentCamera;
                previousCamera.gameObject.SetActive(false);
            }

            currentCamera = newCamera;

            currentCamera.gameObject.SetActive(true);
            cameraNumber = cameras.IndexOf(currentCamera);
        }

        public void DisableCamera(CinemachineCamera camera)
        {
            if(currentCamera == camera)
            {
                if(previousCamera != null)
                {
                    SwitchCamera(previousCamera);
                }
            }
        }

        public void SwitchCameraByIndex(int index)
        {
            SwitchCamera(cameras[index]);
        }
        #endregion
    }
}