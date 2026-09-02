using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private RectTransform rectTransform;
        private static Tween delay;
        public string header;

        [Multiline()]
        public string content;

        [SerializeField] Vector2 _offset;
        [SerializeField] Vector2 _pivot;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            delay = DOVirtual.DelayedCall(0.3f, () =>
            {
                TooltipSystem.Show(content, header, rectTransform.position + new Vector3(_offset.x, _offset.y, 0), _pivot);
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            delay?.Kill();
            TooltipSystem.Hide();
        }
    }
}
