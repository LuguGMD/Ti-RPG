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
        [SerializeField] GameObject[] pushSuperVFXPrefabs;
        [SerializeField] GameObject holdLineVFXPrefab;

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

            // Available Action Display
            ActionsManager.Instance.OnCharacterHoverEnter += ActivateCharacterOutline;
            ActionsManager.Instance.OnCharacterHoverExit += DeactivateCharacterOutline;
            ActionsManager.Instance.OnCharacterActionUsed += DeactivateIndicator;
            ActionsManager.Instance.OnCharacterActionReset += ActivateIndicator;
            ActionsManager.Instance.OnCharacterSelected += LockCharacterOutline;
            ActionsManager.Instance.OnCharacterSelected += PlayCharacterSelectionEffect;
            ActionsManager.Instance.OnCharacterDeselected += UnLockCharacterOutline;
            
            ActionsManager.Instance.OnApresentadorHoverEnter += ActivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorHoverExit += DeactivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorActionCompleted += DeactivateIndicatorApresentador;
            ActionsManager.Instance.OnPlayerTurnStarted += ActivateIndicatorApresentador;
            ActionsManager.Instance.OnApresentadorSelected += LockApresentadorOutline;
            ActionsManager.Instance.OnApresentadorSelected += PlayApresentadorSelectionEffect;
            ActionsManager.Instance.OnApresentadorUIClose += UnLockApresentadorOutline;

            // Supers
            ActionsManager.Instance.OnSpotlightSuperStarted += AddSpotlightSuperPreview;
            ActionsManager.Instance.OnSpotlightSuperEnded += RemoveSpotlightSuperPreview;
            ActionsManager.Instance.OnPushSuperActivated += PlayPushSuperVFX;

            // Hold Line VFX
            ActionsManager.Instance.OnMapLineStuck += PlayHoldLineVFX;
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
            ActionsManager.Instance.OnEnemyDamageTaken -= PlayHitVFX;


            // Available Action Display
            ActionsManager.Instance.OnCharacterHoverEnter -= ActivateCharacterOutline;
            ActionsManager.Instance.OnCharacterHoverExit -= DeactivateCharacterOutline;
            ActionsManager.Instance.OnCharacterActionUsed -= DeactivateIndicator;
            ActionsManager.Instance.OnCharacterActionReset -= ActivateIndicator;
            ActionsManager.Instance.OnCharacterSelected -= LockCharacterOutline;
            ActionsManager.Instance.OnCharacterSelected -= PlayCharacterSelectionEffect;
            ActionsManager.Instance.OnCharacterDeselected -= UnLockCharacterOutline;
            
            ActionsManager.Instance.OnApresentadorHoverEnter -= ActivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorHoverExit -= DeactivateApresentadorOutline;
            ActionsManager.Instance.OnApresentadorActionCompleted -= DeactivateIndicatorApresentador;
            ActionsManager.Instance.OnPlayerTurnStarted -= ActivateIndicatorApresentador;
            ActionsManager.Instance.OnApresentadorSelected -= LockApresentadorOutline;
            ActionsManager.Instance.OnApresentadorSelected -= PlayApresentadorSelectionEffect;
            ActionsManager.Instance.OnApresentadorUIClose -= UnLockApresentadorOutline;

            // Supers
            ActionsManager.Instance.OnSpotlightSuperStarted -= AddSpotlightSuperPreview;
            ActionsManager.Instance.OnSpotlightSuperEnded -= RemoveSpotlightSuperPreview;
            ActionsManager.Instance.OnPushSuperActivated -= PlayPushSuperVFX;

            // Hold Line VFX
            ActionsManager.Instance.OnMapLineStuck -= PlayHoldLineVFX;
        }

        #region Spawn VFX
        void PlaySpawnVFX(StageEntityController entity)
        {
            Instantiate(spawnVFXPrefab, entity.transform.position, Quaternion.identity);
        }
        #endregion

        #region Heal VFX
        void PlayHealVFX(CharacterController entity, float amount)
        {
            Instantiate(healVFXPrefab, entity.transform.position, Quaternion.identity);
        }

        void PlayHealVFXApresentador(float amount)
        {
            Instantiate(healVFXPrefab, CombatManager.Apresentador.transform.position, Quaternion.identity);
        }
#endregion

        #region Hit VFX
        void PlayHitVFX(StageEntityController entity, float amount)
        {
            GameObject vfx = Instantiate(hitVFXPrefab, entity.transform.position, Quaternion.identity, entity.transform);

            Texture2D colorPalette;
            if (entity is CharacterController)
            {
                colorPalette = CombatManager.AttackingEnemy.Info.EntityVfxColorPalette;
                VFXColorPaletteController vfxColorPaletteController = vfx.GetComponent<VFXColorPaletteController>();
                vfxColorPaletteController.SetColorPalette(colorPalette);
            }
            else if (entity is EnemyController)
            {
                colorPalette = CombatManager.SelectedCharacter.Info.EntityVfxColorPalette;
                VFXColorPaletteController vfxColorPaletteController = vfx.GetComponent<VFXColorPaletteController>();
                vfxColorPaletteController.SetColorPalette(colorPalette);
            }
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
            if(selectedCharacter == null) return;
            AvailableActionVFXController vfxController = selectedCharacter.GetComponent<AvailableActionVFXController>();
            vfxController?.UnlockOutline();
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

        #region Supers
        void AddSpotlightSuperPreview()
        {
            spotlightPreviousPosition = Instantiate(spotlightPreviousPositionPrefab, MapManager.Instance.GetWorldPosition(MapManager.SpotlightPosition), Quaternion.identity);
            spotlightPreviousPosition.transform.LookAt(new Vector3(0,spotlightPreviousPosition.transform.position.y,0));
        }

        void RemoveSpotlightSuperPreview()
        {
            Destroy(spotlightPreviousPosition);
        }

        void PlayPushSuperVFX(int upgradeTier)
        {
            Instantiate(pushSuperVFXPrefabs[upgradeTier]);
        }
        #endregion

        #region Selection Effect
        void PlayCharacterSelectionEffect(CharacterController entity)
        {
            entity?.GetComponent<AvailableActionVFXController>().PlaySelectionEffect();
        }

        void PlayApresentadorSelectionEffect()
        {
            apresentadorAvailableActionVFX.PlaySelectionEffect();
        }
        #endregion

        void PlayHoldLineVFX(int line)
        {
            Debug.Log(line);
            EnemyController enemy = CombatManager.AttackingEnemy;
            Instantiate(holdLineVFXPrefab, enemy.transform.position, enemy.transform.rotation);
        }
    }
}
