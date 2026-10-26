using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Combat.UI
{
    public class ApresentadorUIIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            ActionsManager.Instance.OnApresentadorHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ActionsManager.Instance.OnApresentadorHoverExit?.Invoke();

        }
    }
}
