using DG.Tweening;
using UnityEngine;

namespace RPG.Combat
{
    public class SpotlightSparkleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;
        private Renderer[] _renderers;

        public void UpdateSpotlightEffect(bool isOnSpotlight)
        {
            if (isOnSpotlight)
            {
                _effect.Play();
            }
            else
            {
                _effect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (_renderers == null)
            {
                _renderers = GetComponentsInChildren<Renderer>(true);
            }

            float intensity = isOnSpotlight ? 1.0f : 0.0f;

            foreach (Renderer renderer in _renderers)
            {
                if (renderer.materials.Length < 2) continue;

                renderer.materials[1].DOFloat(intensity, "_Fresnel_Intensity", 0.3f);
            }
        }
    }
}
