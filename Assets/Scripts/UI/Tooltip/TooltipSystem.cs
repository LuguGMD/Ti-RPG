using UnityEngine;

namespace RPG
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem instance;
        public Tooltip tooltip;

        public void Awake()
        {
            instance = this;
        }

        public static void Show(string content, string header = "")
        {
            instance.tooltip.SetText(content, header);
            instance.tooltip.gameObject.SetActive(true);
            instance.tooltip.Show();
        }

        public static void Hide()
        {
            instance.tooltip.Hide();
        }
    }
}
