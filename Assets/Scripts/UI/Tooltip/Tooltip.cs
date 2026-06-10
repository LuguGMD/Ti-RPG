using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace RPG.UI.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        [Header("Properties:")]
        public TextMeshProUGUI headerField;
        public TextMeshProUGUI contentField;
        public LayoutElement layoutElement;
        public Image background;
        public float fadeTime;
        public int characterWrapLimit;
        public RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = header;
            }

            contentField.text = content;
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                int headerLength = headerField.text.Length;
                int contentLength = contentField.text.Length;

                layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
            }

            Vector2 position = Mouse.current.position.ReadValue();

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;
            rectTransform.pivot = new Vector2(pivotX, pivotY);

            transform.position = position;
        }

        public void Hide()
        {
            background.DOFade(0, fadeTime).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
            headerField.DOFade(0, fadeTime).From(1);
            contentField.DOFade(0, 0.2f).From(1);
        }

        public void Show()
        {
            background.DOFade(1, fadeTime).From(0);
            headerField.DOFade(1, fadeTime).From(0);
            contentField.DOFade(1, fadeTime).From(0);
        }
    }
}
