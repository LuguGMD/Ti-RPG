using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private BaseCamera targetCamera;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private int selectedCameraIndex = -1;
    private List<BaseCamera> availableCameras = new List<BaseCamera>();

    [ContextMenu("Trocar Câmera")]
    public void SwitchCamera()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("Câmera alvo não configurada em CameraSwitcher");
            return;
        }

        if (CameraManager.Instance == null)
        {
            Debug.LogError("CameraManager não encontrado na cena");
            return;
        }

        CameraManager.Instance.SwitchCamera(targetCamera);
    }

    public void SwitchToCameraByIndex(int index)
    {
        if (CameraManager.Instance == null)
        {
            Debug.LogError("CameraManager não encontrado na cena");
            return;
        }

        CameraManager.Instance.SwitchCameraByIndex(index);
    }

    public void SwitchToCameraByName(string cameraName)
    {
        if (CameraManager.Instance == null)
        {
            Debug.LogError("CameraManager não encontrado na cena");
            return;
        }

        CameraManager.Instance.SwitchCameraByName(cameraName);
    }

    [ContextMenu("Trocar Target da Câmera Atual")]
    public void SetCurrentCameraTarget()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("Target não configurado em CameraSwitcher");
            return;
        }

        if (CameraManager.Instance == null)
        {
            Debug.LogError("CameraManager não encontrado na cena");
            return;
        }

        CameraManager.Instance.SetCurrentCameraTarget(targetTransform);
    }

    public void SetCameraTarget(BaseCamera camera, Transform newTarget)
    {
        if (camera == null || newTarget == null)
        {
            Debug.LogWarning("Câmera ou target não pode ser nulo");
            return;
        }

        if (CameraManager.Instance == null)
        {
            Debug.LogError("CameraManager não encontrado na cena");
            return;
        }

        CameraManager.Instance.SetCameraTarget(camera, newTarget);
    }

    [ContextMenu("Retornar para Câmera Anterior")]
    public void ReturnToPreviousCamera()
    {
        if (CameraManager.Instance == null)
        {
            Debug.LogError("CameraManager não encontrado na cena");
            return;
        }

        CameraManager.Instance.ReturnToPreviousCamera();
    }

    private void OnEnable()
    {
        RefreshAvailableCameras();
    }

    public void RefreshAvailableCameras()
    {
        if (CameraManager.Instance == null)
        {
            Debug.LogWarning("CameraManager não encontrado na cena");
            availableCameras.Clear();
            return;
        }

        availableCameras = CameraManager.Instance.GetAllCameras();
        Debug.Log($"[CameraSwitcher] {availableCameras.Count} câmeras encontradas");
    }

    public void SelectCameraByIndex(int index)
    {
        if (index < 0 || index >= availableCameras.Count)
        {
            Debug.LogError($"Índice inválido: {index}. Total de câmeras: {availableCameras.Count}");
            return;
        }

        selectedCameraIndex = index;
        targetCamera = availableCameras[index];
        Debug.Log($"Câmera selecionada: {targetCamera.gameObject.name}");
    }

    [ContextMenu("Trocar para Câmera Selecionada")]
    public void SwitchToSelectedCamera()
    {
        if (selectedCameraIndex < 0 || selectedCameraIndex >= availableCameras.Count)
        {
            Debug.LogWarning("Nenhuma câmera selecionada");
            return;
        }

        targetCamera = availableCameras[selectedCameraIndex];
        SwitchCamera();
    }

    public string[] GetAvailableCameraNames()
    {
        RefreshAvailableCameras();

        if (availableCameras.Count == 0)
        {
            return new string[] { "Nenhuma câmera disponível" };
        }

        string[] names = new string[availableCameras.Count];
        for (int i = 0; i < availableCameras.Count; i++)
        {
            names[i] = $"[{i}] {availableCameras[i].gameObject.name}";
        }

        return names;
    }

    public int GetSelectedCameraIndex()
    {
        return selectedCameraIndex;
    }

    public BaseCamera GetSelectedCamera()
    {
        if (selectedCameraIndex >= 0 && selectedCameraIndex < availableCameras.Count)
        {
            return availableCameras[selectedCameraIndex];
        }

        return null;
    }

    [ContextMenu("Listar Todas as Câmeras")]
    public void ListAllCameras()
    {
        RefreshAvailableCameras();

        if (availableCameras.Count == 0)
        {
            Debug.LogWarning("Nenhuma câmera disponível na cena");
            return;
        }

        Debug.Log("=== Câmeras Disponíveis ===");
        for (int i = 0; i < availableCameras.Count; i++)
        {
            string isCurrent = availableCameras[i] == CameraManager.Instance.GetCurrentCamera() ? " (ATIVA)" : "";
            string isSelected = i == selectedCameraIndex ? " (SELECIONADA)" : "";
            Debug.Log($"[{i}] {availableCameras[i].gameObject.name}{isCurrent}{isSelected}");
        }
    }
}

#if UNITY_EDITOR


[CustomEditor(typeof(CameraSwitcher))]
public class CameraSwitcherEditor : Editor
{
    private int displayIndex = 0;
    private string[] cameraNames = new string[] { };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Seletor de Câmera", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        if (GUILayout.Button("Atualizar Lista de Câmeras"))
        {
            RefreshList();
        }

        EditorGUILayout.Space(5);

        int previousIndex = displayIndex;
        displayIndex = EditorGUILayout.Popup("Câmera", displayIndex, cameraNames);

        if (displayIndex != previousIndex || cameraNames.Length == 0)
        {
            RefreshList();
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Selecionar Câmera"))
        {
            CameraSwitcher switcher = (CameraSwitcher)target;
            switcher.SelectCameraByIndex(displayIndex);
            EditorUtility.SetDirty(switcher);
        }

        if (GUILayout.Button("Trocar para Câmera Selecionada"))
        {
            CameraSwitcher switcher = (CameraSwitcher)target;
            switcher.SwitchToSelectedCamera();
        }

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Listar Câmeras (Console)"))
        {
            CameraSwitcher switcher = (CameraSwitcher)target;
            switcher.ListAllCameras();
        }
    }

    private void RefreshList()
    {
        CameraSwitcher switcher = (CameraSwitcher)target;
        switcher.RefreshAvailableCameras();
        cameraNames = switcher.GetAvailableCameraNames();

        if (displayIndex < 0 || displayIndex >= cameraNames.Length)
            displayIndex = 0;
    }
}

#endif
