using UnityEngine;

namespace RPG
{
    public class PreviewFillHandler : MonoBehaviour
    {
        [SerializeField] GameObject[] fills;

        void Start()
        {
            DisableFills();
        }

        [ContextMenu("Enable Fills")]
        public void EnableFills()
        {
            foreach (GameObject fill in fills)
            {
                fill.SetActive(true);
            }
        }
        
        [ContextMenu("Disable Fills")]
        public void DisableFills()
        {
            foreach (GameObject fill in fills)
            {
                fill.SetActive(false);
            }
        }
    }
}
