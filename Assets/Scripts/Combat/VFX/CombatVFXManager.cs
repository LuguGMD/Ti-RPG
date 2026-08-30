using RPG.Combat;
using UnityEngine;

namespace RPG.Combat.VFX
{
    public class CombatVFXManager : MonoBehaviour
    {
        [SerializeField] GameObject spawnVFX;
        [SerializeField] GameObject healVFX;

        void OnEnable()
        {
            ActionsManager.Instance.OnStageEntityCreated += PlaySpawnVFX;
            ActionsManager.Instance.OnStageEntityDefeated += PlaySpawnVFX;
            ActionsManager.Instance.OnCharacterHealed += PlayHealVFX;
            ActionsManager.Instance.OnApresentadorHealed += PlayHealVFXApresentador;
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

        void PlayHealVFX(CharacterController entity)
        {
            Instantiate(healVFX, entity.transform.position, Quaternion.identity);
        }

        void PlayHealVFXApresentador()
        {
            Instantiate(healVFX, CombatManager.Apresentador.transform.position, Quaternion.identity);
        }
    }
}
