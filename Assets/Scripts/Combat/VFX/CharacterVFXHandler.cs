using UnityEngine;

namespace RPG
{
    public class CharacterVFXHandler : MonoBehaviour
    {
        [SerializeField] GameObject trailObject;

        TrailRenderer trail;
        ParticleSystem[] trailParticles;

        private void Start()
        {
            if (trailObject != null)
            {
                trail = trailObject.GetComponent<TrailRenderer>();
                trailParticles = trailObject.GetComponentsInChildren<ParticleSystem>();
            }
        }

        public void PlayAoeVFX(GameObject aoeVFX)
        {
            GameObject vfx = Instantiate(aoeVFX, transform.position, Quaternion.identity);
            vfx.transform.LookAt(Vector3.zero);
        }

        public void PlaySlashVFX(GameObject slashVFX)
        {
            Instantiate(slashVFX, transform.position, transform.rotation, transform);
        }

        public void PlayBasicEnemySlashVFX(GameObject slashVFX)
        {
            BasicEnemySlashVFXController slashVFXMaster = slashVFX.GetComponent<BasicEnemySlashVFXController>();

            if (transform.parent.name.Contains("Anger"))
            {
                Instantiate(slashVFXMaster.slashes[0], transform.position, transform.rotation, transform);
            }
            else if (transform.parent.name.Contains("Fear"))
            {
                Instantiate(slashVFXMaster.slashes[1], transform.position, transform.rotation, transform);
            }
            else if (transform.parent.name.Contains("Sadness"))
            {
                Instantiate(slashVFXMaster.slashes[2], transform.position, transform.rotation, transform);
            }
        }

        public void SetTrailEmission(int emit)
        {
            if (emit == 1)
            {
                trail.emitting = true;
                foreach(ParticleSystem particle in trailParticles)
                {
                    particle.Play();
                }
            }
            else
            {
                trail.emitting = false;
                foreach (ParticleSystem particle in trailParticles)
                {
                    particle.Stop();
                }
            }
        }
    }
}
