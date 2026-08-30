using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(_originalScale * _hoverScale, _animationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_originalScale, _animationDuration);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(_originalScale * _clickScale, 0.08f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(_originalScale * _hoverScale, _animationDuration);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}