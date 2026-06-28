using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private static Tween delay;
        public string header;

        [Multiline()]
        public string content;

        public void OnPointerEnter(PointerEventData eventData)
        {
            delay = DOVirtual.DelayedCall(0.5f, () =>
            {
                TooltipSystem.Show(content, header);
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            delay?.Kill();
            TooltipSystem.Hide();
        }
    }
}
