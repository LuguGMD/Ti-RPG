using Lugu.Singleton;
using UnityEngine;

namespace RPG.UI.Tooltip
{
    public class TooltipSystem : SingletonMono<TooltipSystem>
    {
        public Tooltip tooltip;

        public static void Show(string content, string header = "", Vector2 position = default, Vector2 pivot = default)
        {
            Instance.tooltip.SetText(content, header);
            Instance.tooltip.gameObject.SetActive(true);
            Instance.tooltip.rectTransform.position = position;
            Instance.tooltip.rectTransform.pivot = pivot;
            Instance.tooltip.Show();
        }

        public static void Hide()
        {
            Instance.tooltip.Hide();
        }
    }
}
