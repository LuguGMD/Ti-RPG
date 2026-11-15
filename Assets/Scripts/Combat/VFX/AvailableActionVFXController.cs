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

        [SerializeField] Transform selectionBeam;
        [SerializeField] float beamRiseDuration = 0.5f;
        [SerializeField] float beamHoldDuration = 0.7f;
        [SerializeField] float beamFallDuration = 0.4f;

        Sequence beamSequence;

        public void PlaySelectionEffect()
        {
            beamSequence?.Kill();

            selectionBeam.localScale = new Vector3(1.5f, 0f, 1.5f);
            selectionBeam.gameObject.SetActive(true);

            beamSequence = DOTween.Sequence()
                .SetLink(gameObject)
                .Append(selectionBeam.DOScaleY(1f, beamRiseDuration).SetEase(Ease.OutCubic))
                .AppendInterval(beamHoldDuration)
                .Append(selectionBeam.DOScaleY(0f, beamFallDuration).SetEase(Ease.InCubic))
                .OnComplete(() => selectionBeam.gameObject.SetActive(false));
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
