using Lugu.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Camera
{
    public class CameraManager : SingletonMono<CameraManager>
    {
        [SerializeField]
        private List<GameObject> cameras = new List<GameObject>();

        [SerializeField]
        [Tooltip("Tempo de transição")]
        private float transitionDelay = 1f;

        private GameObject currentCamera;
        private GameObject previousCamera;

        [SerializeField]
        [Tooltip("Selecione a câmera pelo índice no inspetor")]
        private int cameraNumber = 0;

        #region Properties

        public GameObject CurrentCamera { get { return currentCamera; } }
        public GameObject PreviousCamera { get { return previousCamera; } }
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

            foreach (GameObject cam in cameras)
            {
                cam.SetActive(false);
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

        public void SwitchCamera(GameObject newCamera)
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
                previousCamera.SetActive(false);
            }

            currentCamera = newCamera;

            currentCamera.SetActive(true);
            cameraNumber = cameras.IndexOf(currentCamera);
        }

        public void DisableCamera(GameObject camera)
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

        public void SwitchCameraWithTransition(int initialCameraIndex)
        {

            if (initialCameraIndex % 2 == 0)
            {
                int nextCameraIndex = initialCameraIndex + 1;
                if (nextCameraIndex < cameras.Count)
                {
                    StartCoroutine(TransitionCameras(initialCameraIndex, nextCameraIndex));
                }
                else
                {
                    SwitchCamera(cameras[initialCameraIndex]);
                }
            }
            else
            {
                SwitchCamera(cameras[initialCameraIndex]);
            }
        }

        private IEnumerator TransitionCameras(int fromCameraIndex, int toCameraIndex)
        {
            SwitchCamera(cameras[fromCameraIndex]);

            yield return new WaitForSeconds(transitionDelay);

            SwitchCamera(cameras[toCameraIndex]);
        }

        public void SwitchCameraForLevel(int levelIndex)
        {
            int initialCameraIndex = levelIndex * 2;
            SwitchCameraWithTransition(initialCameraIndex);
        }
        #endregion
    }
}