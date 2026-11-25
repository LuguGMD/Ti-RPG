using DG.Tweening;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace RPG.Combat
{
    public class SpotlightSparkleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;
        private Renderer[] _renderers;
        private StageEntityScriptable entity;

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

            if (entity == null)
            {
                entity = GetComponent<StageEntityController>().Info;
            }

            float intensity = isOnSpotlight ? 1.0f : 0.0f;
            float duration = isOnSpotlight ? 0.01f : 0.3f;
            Vector4 pulseRange = isOnSpotlight ? new Vector4(0.6f, 0.8f, 0, 0) : Vector4.zero;

            foreach (Renderer renderer in _renderers)
            {
                if (renderer == null) continue;
                if (renderer.materials.Length < 2) continue;

                renderer.materials[1].SetColor("_Color", entity.EntitySpotlightHighlightColor);
                renderer.materials[1].DOFloat(intensity, "_Fresnel_Intensity", duration).SetDelay(0.5f).OnComplete(() =>
                    renderer.materials[1].DOVector(pulseRange, "_Pulse_Range", 0.7f));
            }
        }
    }
}
