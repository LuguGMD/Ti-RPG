using RPG.Combat;
using UnityEngine;

namespace RPG.Combat.VFX
{
    public class CombatVFXManager : MonoBehaviour
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
