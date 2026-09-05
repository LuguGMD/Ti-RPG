using UnityEngine;

namespace RPG
{
    public class AvailableActionVFXController : MonoBehaviour
    {
        [SerializeField] GameObject[] canOutline;
        [SerializeField] GameObject indicator;

        bool actionAvailable = true;
        bool lockOutline = false;

        public void ActivateOutline()
        {
            if (actionAvailable)
            {
                foreach (GameObject g in canOutline)
                {
                    g.layer = LayerMask.NameToLayer("Outlined");
                }
            }
        }

        public void DeactivateOutline()
        {
            if (!lockOutline)
            {
                foreach (GameObject g in canOutline)
                {
                    g.layer = LayerMask.NameToLayer("Default");
                }  
            }
              
        }

        public void ActivateIndicator()
        {
            indicator.SetActive(true);
            actionAvailable = true;
        }

        public void DeactivateIndicator()
        {
            indicator.SetActive(false);
            actionAvailable = false;
        }

        public void LockOutline()
        {
            lockOutline = true;
        }

        public void UnlockOutline()
        {
            lockOutline = false;
        }
    }
}
