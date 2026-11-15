using UnityEngine;
using UnityEngine.Events;

public class ConfirmationRequester : MonoBehaviour
{
    [SerializeField] private ConfirmationPanel panel;
    [SerializeField, TextArea] private string message;
    [SerializeField] private UnityEvent onConfirmed;

    public void Request()
    {
        panel.Show(message, onConfirmed.Invoke);
    }
}