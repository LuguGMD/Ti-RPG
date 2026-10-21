
using UnityEngine;
using UnityEngine.UI;

public class ProgressionPanelZoom : MonoBehaviour
{
    [Header("Referências da UI")]
    [SerializeField] private RectTransform zoomTarget;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Configurações do Zoom")]
    [SerializeField] private float minZoom = 0.75f;
    [SerializeField] private float maxZoom = 1.75f;
    [SerializeField] private float zoomStep = 0.1f;
    [SerializeField] private float mouseZoomSpeed = 0.15f;

    [Header("Movimentação pelo Teclado")]
    [SerializeField] private float keyboardPanSpeed = 0.5f;

    private float currentZoom = 1f;

    public float CurrentZoom => currentZoom;

    private void Awake()
    {
        if (zoomTarget == null)
        {
            Debug.LogError(
                "UpgradePanelZoom: atribua o Content em Zoom Target.",
                this
            );
            enabled = false;
            return;
        }

        minZoom = Mathf.Max(0.01f, minZoom);
        maxZoom = Mathf.Max(minZoom, maxZoom);
        zoomStep = Mathf.Max(0.01f, zoomStep);

        zoomTarget.pivot = new Vector2(0.5f, 0.5f);

        if (scrollRect != null)
        {
            scrollRect.scrollSensitivity = 0f;
        }

        currentZoom = Mathf.Clamp(1f, minZoom, maxZoom);
        ApplyZoom();
    }

    private void Update()
    {
        HandleMouseZoom();
        HandleKeyboardZoom();
        HandleKeyboardPan();
    }

    private void HandleMouseZoom()
    {
        if (viewport == null)
            return;

        if (!RectTransformUtility.RectangleContainsScreenPoint(
            viewport,
            Input.mousePosition,
            null))
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            SetZoom(currentZoom + mouseZoomSpeed);
        else if (scroll < 0f)
            SetZoom(currentZoom - mouseZoomSpeed);
    }

    private void HandleKeyboardZoom()
    {
        if (Input.GetKeyDown(KeyCode.Plus) ||
            Input.GetKeyDown(KeyCode.Equals) ||
            Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ZoomIn();
        }

        if (Input.GetKeyDown(KeyCode.Minus) ||
            Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ZoomOut();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) ||
            Input.GetKeyDown(KeyCode.Keypad0))
        {
            ResetZoom();
        }
    }

    public void ZoomIn()
    {
        SetZoom(currentZoom + zoomStep);
    }

    public void ZoomOut()
    {
        SetZoom(currentZoom - zoomStep);
    }

    public void ResetZoom()
    {
        SetZoom(1f);
    }

    public void SetZoom(float value)
    {
        currentZoom = Mathf.Clamp(value, minZoom, maxZoom);
        ApplyZoom();
    }

    private void ApplyZoom()
    {
        if (zoomTarget == null)
            return;

        zoomTarget.localScale = new Vector3(
            currentZoom,
            currentZoom,
            1f
        );
    }


    private void HandleKeyboardPan()
    {
        if (scrollRect == null)
            return;

        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow))
            horizontal -= 1f;

        if (Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow))
            horizontal += 1f;

        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow))
            vertical += 1f;

        if (Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow))
            vertical -= 1f;

        float movement = keyboardPanSpeed * Time.unscaledDeltaTime;

        if (scrollRect.horizontal)
        {
            scrollRect.horizontalNormalizedPosition =
                Mathf.Clamp01(
                    scrollRect.horizontalNormalizedPosition
                    + horizontal * movement
                );
        }

        if (scrollRect.vertical)
        {
            scrollRect.verticalNormalizedPosition =
                Mathf.Clamp01(
                    scrollRect.verticalNormalizedPosition
                    + vertical * movement
                );
        }
    }

}
