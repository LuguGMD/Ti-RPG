using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private Action onConfirm;

    private void Awake()
    {
        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Hide);
    }

    private void OnDestroy()
    {
        confirmButton.onClick.RemoveListener(Confirm);
        cancelButton.onClick.RemoveListener(Hide);
    }

    public void Show(string message, Action onConfirm)
    {
        messageText.text = message;
        this.onConfirm = onConfirm;
        gameObject.SetActive(true);
    }

    private void Confirm()
    {
        var action = onConfirm;
        Hide();
        action?.Invoke();
    }

    private void Hide()
    {
        onConfirm = null;
        gameObject.SetActive(false);
    }
}