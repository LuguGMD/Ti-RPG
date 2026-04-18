using UnityEngine;
using System;

public class BaseCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Component virtualCamera;
    private Type cinemachineType;

    private void Awake()
    {
        try
        {
            cinemachineType = System.Type.GetType("Cinemachine.CinemachineVirtualCamera");
            if (cinemachineType != null)
            {
                virtualCamera = GetComponent(cinemachineType);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"CinemachineVirtualCamera não encontrada em {gameObject.name}. Certifique-se de que o Cinemachine está instalado. Erro: {e.Message}");
        }
    }

    public GameObject GetCameraObject()
    {
        return gameObject;
    }

    public void Activate()
    {
        gameObject.SetActive(true);

        if (virtualCamera != null && target != null)
        {
            SetCinemachineTarget(target);
        }

        Debug.Log($"Câmera ativada: {gameObject.name}");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Debug.Log($"Câmera desativada: {gameObject.name}");
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null)
        {
            Debug.LogWarning("Target não pode ser nulo");
            return;
        }

        target = newTarget;

        if (gameObject.activeSelf && virtualCamera != null)
        {
            SetCinemachineTarget(target);
            Debug.Log($"Target da câmera '{gameObject.name}' alterado para: {newTarget.name}");
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    private void SetCinemachineTarget(Transform newTarget)
    {
        if (virtualCamera == null || cinemachineType == null)
            return;

        try
        {
            var followProperty = cinemachineType.GetProperty("Follow");
            var lookAtProperty = cinemachineType.GetProperty("LookAt");

            if (followProperty != null && followProperty.CanWrite)
                followProperty.SetValue(virtualCamera, newTarget);

            if (lookAtProperty != null && lookAtProperty.CanWrite)
                lookAtProperty.SetValue(virtualCamera, newTarget);
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao definir target da câmera: {e.Message}");
        }
    }
}
