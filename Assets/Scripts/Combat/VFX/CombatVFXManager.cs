using RPG.Combat;
using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat.VFX
{
    public class CombatVFXManager : MonoBehaviour
    {
        [SerializeField] GameObject spawnVFXPrefab;
        [SerializeField] GameObject healVFXPrefab;
        [SerializeField] GameObject hitVFXPrefab;
        [SerializeField] GameObject spotlightPreviousPositionPrefab;

        CharacterController selectedCharacter;
        GameObject spotlightPreviousPosition;
        AvailableActionVFXController apresentadorAvailableActionVFX;

        void Start()
        {
            apresentadorAvailableActionVFX = FindAnyObjectByType<ApresentadorController>(FindObjectsInactive.Include).GetComponent<AvailableActionVFXController>();
        }

        void OnEnable()
        {
            // Spawn VFX
            ActionsManager.Instance.OnStageEntityCreated += PlaySpawnVFX;
            ActionsManager.Instance.OnStageEntityDefeated += PlaySpawnVFX;

            // Heal VFX
            ActionsManager.Instance.OnCharacterHealed += PlayHealVFX;
            ActionsManager.Instance.OnApresentadorHealed += PlayHealVFXApresentador;

            // Hit VFX
            ActionsManager.Instance.OnCharacterDamageTaken += PlayHitVFX;
            ActionsManager.Instance.OnEnemyDamageTaken += PlayHitVFX;


            // Spotlight Super
            ActionsManager.Instance.OnSpotlightSuperStarted += AddSpotlightSuperPreview;
            ActionsManager.Instance.OnSpotlightSuperEnded += RemoveSpotlightSuperPreview;
        }

        void OnDisable()
        {
            // Spawn VFX
            ActionsManager.Instance.OnStageEntityCreated -= PlaySpawnVFX;
            ActionsManager.Instance.OnStageEntityDefeated -= PlaySpawnVFX;

            // Heal VFX
            ActionsManager.Instance.OnCharacterHealed -= PlayHealVFX;
            ActionsManager.Instance.OnApresentadorHealed -= PlayHealVFXApresentador;

            // Hit VFX
            ActionsManager.Instance.OnCharacterDamageTaken -= PlayHitVFX;

            // Spotlight Super
            ActionsManager.Instance.OnSpotlightSuperStarted -= AddSpotlightSuperPreview;
            ActionsManager.Instance.OnSpotlightSuperEnded -= RemoveSpotlightSuperPreview;
        }

        #region Spawn VFX
        void PlaySpawnVFX(StageEntityController entity)
        {
            Instantiate(spawnVFXPrefab, entity.transform.position, Quaternion.identity);
        }
        #endregion

        #region Heal VFX
        void PlayHealVFX(CharacterController entity)
        {
            Instantiate(healVFXPrefab, entity.transform.position, Quaternion.identity);
        }

        void PlayHealVFXApresentador()
        {
            Instantiate(healVFXPrefab, CombatManager.Apresentador.transform.position, Quaternion.identity);
        }
#endregion

        #region Hit VFX
        void PlayHitVFX(StageEntityController entity)
        {
            Instantiate(hitVFXPrefab, entity.transform.position, Quaternion.identity, entity.transform);
        }
        #endregion


        #region Spotlight Super Preview
        void AddSpotlightSuperPreview()
        {
            spotlightPreviousPosition = Instantiate(spotlightPreviousPositionPrefab, MapManager.Instance.GetWorldPosition(MapManager.SpotlightPosition), Quaternion.identity);
            spotlightPreviousPosition.transform.LookAt(new Vector3(0,spotlightPreviousPosition.transform.position.y,0));
        }

        void RemoveSpotlightSuperPreview()
        {
            Destroy(spotlightPreviousPosition);
        }
        #endregion

    }
}
