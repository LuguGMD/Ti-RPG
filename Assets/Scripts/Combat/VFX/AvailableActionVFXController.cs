using DG.Tweening;
using UnityEngine;

namespace RPG
{
    public class AvailableActionVFXController : MonoBehaviour
    {
        [SerializeField] GameObject[] canOutline;
        [SerializeField] GameObject indicator;

        bool actionAvailable = true;
        bool lockOutline = false;

        [SerializeField] GameObject selectionVFX;
        [SerializeField] float selectionVFXDuration;

        Tween selectionVFXTimer;

        public void PlaySelectionEffect()
        {
            Debug.Log("Playing selection effect");
            if (selectionVFX == null) return;

            selectionVFXTimer?.Kill();

            selectionVFX.SetActive(false);
            selectionVFX.SetActive(true);

            selectionVFXTimer = DOVirtual.DelayedCall(selectionVFXDuration, () => selectionVFX.SetActive(false))
                .SetLink(gameObject);
        }

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
            UnlockOutline();
            DeactivateOutline();
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
