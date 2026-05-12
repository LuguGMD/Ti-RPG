using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();

    private GameObject currentCamera;

    [SerializeField]
    [Tooltip("Selecione a câmera pelo índice no inspetor")]
    private int cameraNumber = 0;

    #region Unity Methods
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

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

        currentCamera = cameras[0];
        currentCamera.SetActive(true);

        cameraNumber = 0;
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
            currentCamera.SetActive(false);

        newCamera.SetActive(true);
        currentCamera = newCamera;
        cameraNumber = cameras.IndexOf(newCamera);
    }

    public void SwitchCameraByIndex(int index)
    {
        SwitchCamera(cameras[index]);
    }

    public GameObject GetCurrentCamera()
    {
        return currentCamera;
    }

    public int GetCurrentCameraIndex()
    {
        return cameras.IndexOf(currentCamera);
    }
    #endregion
}