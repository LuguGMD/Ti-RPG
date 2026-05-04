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
    private int inspectorCameraIndex = 0;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Start();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //apenas para teste de trocar a camera no inspctor
    void OnValidate()
    {
        
        if (cameras.Count > 0)
        {
            inspectorCameraIndex = Mathf.Clamp(inspectorCameraIndex, 0, cameras.Count - 1);

            if (currentCamera != cameras[inspectorCameraIndex])
            {
                SwitchCamera(cameras[inspectorCameraIndex]);
            }
        }
    }

    void Start()
    {
        if (cameras.Count == 0)
        {
            Debug.LogWarning("CameraManager: Nenhuma câmera foi adicionada à lista!");
            return;
        }

        foreach (var cam in cameras)
        {
            cam.SetActive(false);
        }

        inspectorCameraIndex = 0;
        currentCamera = cameras[0];
        currentCamera.SetActive(true);
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
        inspectorCameraIndex = cameras.IndexOf(newCamera);
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
}