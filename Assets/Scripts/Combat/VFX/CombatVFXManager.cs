using RPG.Combat;
using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat.VFX
{
    public class CombatVFXManager : MonoBehaviour
    {
        [SerializeField] GameObject spawnVFXPrefab;
        [SerializeField] GameObject healVFXPrefab;
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

            // Available Action Display
            ActionsManager.Instance.OnCharacterHoverEnter += ActivateCharacterOutline;
            ActionsManager.Instance.OnCharacterHoverExit += DeactivateCharacterOutline;
            ActionsManager.Instance.OnCharacterActionUsed += DeactivateIndicator;
            ActionsManager.Instance.OnCharacterActionReset += ActivateIndicator;
            ActionsManager.Instance.OnApresentadorActionCompleted += DeactivateIndicatorApresentador;
            ActionsManager.Instance.OnPlayerTurnStarted += ActivateIndicatorApresentador;
            // ActionsManager.Instance.OnCharacterSelected += LockCharacterOutline;
            // ActionsManager.Instance.OnCharacterDeselected += UnLockCharacterOutline;

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

            // Available Action Display
            ActionsManager.Instance.OnCharacterHoverEnter -= ActivateCharacterOutline;
            ActionsManager.Instance.OnCharacterHoverExit -= DeactivateCharacterOutline;
            ActionsManager.Instance.OnCharacterActionUsed -= DeactivateIndicator;
            ActionsManager.Instance.OnCharacterActionReset -= ActivateIndicator;
            ActionsManager.Instance.OnApresentadorActionCompleted -= DeactivateIndicatorApresentador;
            ActionsManager.Instance.OnPlayerTurnStarted -=ActivateIndicatorApresentador;
            // ActionsManager.Instance.OnCharacterSelected -= LockCharacterOutline;
            // ActionsManager.Instance.OnCharacterDeselected -= UnLockCharacterOutline;

            // Spotlight Super
            ActionsManager.Instance.OnSpotlightSuperStarted -= AddSpotlightSuperPreview;
            ActionsManager.Instance.OnSpotlightSuperEnded -= RemoveSpotlightSuperPreview;
        }

        void PlaySpawnVFX(StageEntityController entity)
        {
            Instantiate(spawnVFXPrefab, entity.transform.position, Quaternion.identity);
        }

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

#region Available Action Indicator
        void ActivateCharacterOutline(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().ActivateOutline();
        }

        void LockCharacterOutline(CharacterController entity)
        {
            selectedCharacter = entity;
            selectedCharacter?.GetComponent<AvailableActionVFXController>().LockOutline();
        }

        void UnLockCharacterOutline()
        {
            selectedCharacter?.GetComponent<AvailableActionVFXController>().UnlockOutline();
        }

        void DeactivateCharacterOutline(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().DeactivateOutline();
        }

        void ActivateIndicator(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().ActivateIndicator();
        }

        void DeactivateIndicator(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().DeactivateIndicator();
        }

        void ActivateIndicatorApresentador()
        {
            apresentadorAvailableActionVFX.ActivateIndicator();
        }

        void DeactivateIndicatorApresentador()
        {
            apresentadorAvailableActionVFX.DeactivateIndicator();
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
