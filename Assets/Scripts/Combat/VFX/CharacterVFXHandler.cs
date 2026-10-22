using UnityEngine;

namespace RPG
{
    public class CharacterVFXHandler : MonoBehaviour
    {
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
    }
}
