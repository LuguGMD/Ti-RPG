using RPG.Combat;
using UnityEngine;

namespace RPG
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] GameObject spawnVFX;

        void OnEnable()
        {
            ActionsManager.Instance.OnStageEntityCreated += PlaySpawnVFX;
            ActionsManager.Instance.OnStageEntityDefeated += PlaySpawnVFX;
        }

        void OnDisable()
        {
            ActionsManager.Instance.OnStageEntityCreated -= PlaySpawnVFX;
            ActionsManager.Instance.OnStageEntityDefeated -= PlaySpawnVFX;
        }

        void PlaySpawnVFX(StageEntityController entity)
        {
            Instantiate(spawnVFX, entity.transform.position, Quaternion.identity);
        }
    }
}
