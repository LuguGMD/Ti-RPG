using UnityEngine;

public class UpgradeZoom : MonoBehaviour
{
    public RectTransform graphContainer;

    [Header("Configuração de Zoom")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2.0f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.001f) return;

        float currentScale = graphContainer.localScale.x;
        float newScale = Mathf.Clamp(currentScale + scroll * zoomSpeed * currentScale, minZoom, maxZoom);
        graphContainer.localScale = Vector3.one * newScale;
    }
}