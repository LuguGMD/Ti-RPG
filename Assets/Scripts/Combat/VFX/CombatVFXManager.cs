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
            ActionsManager.Instance.OnCharacterSelected += LockCharacterOutline;
            ActionsManager.Instance.OnCharacterDeselected += UnLockCharacterOutline;
            
            ActionsManager.Instance.OnApresentadorHoverEnter += ActivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorHoverExit += DeactivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorActionCompleted += DeactivateIndicatorApresentador;
            ActionsManager.Instance.OnPlayerTurnStarted += ActivateIndicatorApresentador;
            ActionsManager.Instance.OnApresentadorSelected += LockApresentadorOutline;
            ActionsManager.Instance.OnApresentadorUIClose += UnLockApresentadorOutline;

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
            ActionsManager.Instance.OnCharacterSelected -= LockCharacterOutline;
            ActionsManager.Instance.OnCharacterDeselected -= UnLockCharacterOutline;
            
            ActionsManager.Instance.OnApresentadorHoverEnter -= ActivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorHoverExit -= DeactivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorActionCompleted -= DeactivateIndicatorApresentador;
            ActionsManager.Instance.OnPlayerTurnStarted -= ActivateIndicatorApresentador;
            ActionsManager.Instance.OnApresentadorSelected -= LockApresentadorOutline;
            ActionsManager.Instance.OnApresentadorUIClose -= UnLockApresentadorOutline;

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
        
        // Circenses
        void ActivateCharacterOutline(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().ActivateOutline();
        }

        void DeactivateCharacterOutline(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().DeactivateOutline();
        }

        void LockCharacterOutline(CharacterController entity)
        {
            selectedCharacter = entity;
            selectedCharacter?.GetComponent<AvailableActionVFXController>().LockOutline();
            ActivateCharacterOutline(selectedCharacter);
        }

        void UnLockCharacterOutline()
        {
            selectedCharacter?.GetComponent<AvailableActionVFXController>().UnlockOutline();
            DeactivateCharacterOutline(selectedCharacter);
        }

        void ActivateIndicator(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().ActivateIndicator();
        }

        void DeactivateIndicator(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().DeactivateIndicator();
        }

        // Apresentador
        void ActivateApresentadorOutline()
        {
            apresentadorAvailableActionVFX.ActivateOutline();
        }

        void DeactivateApresentadorOutline()
        {
            apresentadorAvailableActionVFX.DeactivateOutline();
        }

        void LockApresentadorOutline()
        {
            //selectedCharacter = entity;
            apresentadorAvailableActionVFX.LockOutline();
            ActivateApresentadorOutline();
        }

        void UnLockApresentadorOutline()
        {
            apresentadorAvailableActionVFX.UnlockOutline();
            DeactivateApresentadorOutline();
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
