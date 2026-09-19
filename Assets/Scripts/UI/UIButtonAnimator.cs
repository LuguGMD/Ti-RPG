using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

namespace RPG.UI
{
    public class UIButtonAnimator : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private float _hoverScale = 1.1f;
        [SerializeField] private float _clickScale = 0.95f;
        [SerializeField] private float _animationDuration = 0.15f;
        private Button _button;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button != null && !_button.interactable) return;

            transform.DOScale(_originalScale * _hoverScale, _animationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_originalScale, _animationDuration);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_button != null && !_button.interactable) return;

            transform.DOScale(_originalScale * _clickScale, 0.08f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_button != null && !_button.interactable) return;

            transform.DOScale(_originalScale * _hoverScale, _animationDuration);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void OnDisable()
        {
            transform.DOKill();
            transform.localScale = _originalScale;
        }
    }
}