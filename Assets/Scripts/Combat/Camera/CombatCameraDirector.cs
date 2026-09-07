using Lugu.Singleton;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG.Combat.Cameras
{
    [DefaultExecutionOrder(-40)]
    public class CombatCameraDirector : SingletonMono<CombatCameraDirector>
    {
        private const int StandbyPriority = 0;
        private const int PlayerPriority = 10;
        private const int EnemyLivePriority = 20;

        [Header("Cameras")]
        [SerializeField] private CinemachineCamera _playerCamera;
        [SerializeField] private CinemachineCamera _enemyCamera;
        [SerializeField] private CinemachineInputAxisController _playerAxisController;

        [Header("Focus")]
        [SerializeField] private CombatCameraFocusTarget _playerFocus;
        [SerializeField] private CombatCameraFocusTarget _enemyFocus;
        [SerializeField] private bool _focusApresentadorOnSelect = true;

        [Header("Timing")]
        [Tooltip("Todos os tempos sao divididos por CombatManager.CombatSpeed.")]
        [SerializeField] private float _baseBlendTime = 0.5f;
        [SerializeField] private float _enemyFocusSettleTime = 0.4f;
        [SerializeField] private float _returnSettleTime = 0.15f;

        private CinemachineBrain _brain;
        private bool _isEnemyViewLive;
        private static float CombatSpeed
        {
            get { return CombatManager.Instance != null ? CombatManager.CombatSpeed : 1f; }
        }
        protected override void Awake()
        {
            base.Awake();

            if (Instance != this) return;

            UnityEngine.Camera main = UnityEngine.Camera.main;

            if (main != null) _brain = main.GetComponent<CinemachineBrain>();

            ApplyBlendTime();
        }
        private void Start()
        {
            if (Instance != this) return;

            _playerCamera.Priority = PlayerPriority;
            _enemyCamera.Priority = StandbyPriority;
            _isEnemyViewLive = false;

            _playerFocus.SetTarget(null, true);
        }
        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterSelected += HandleCharacterSelected;
            ActionsManager.Instance.OnCharacterDeselected += HandleCharacterDeselected;
            ActionsManager.Instance.OnApresentadorSelected += HandleApresentadorSelected;
            ActionsManager.Instance.OnStageEntityDefeated += HandleStageEntityDefeated;
            ActionsManager.Instance.OnCombatWon += HandleCombatEnded;
            ActionsManager.Instance.OnCombatLost += HandleCombatEnded;
            ActionsManager.Instance.OnPauseToggle += HandlePauseToggle;
            ActionsManager.Instance.OnCombatSpeedChanged += ApplyBlendTime;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterSelected -= HandleCharacterSelected;
            ActionsManager.Instance.OnCharacterDeselected -= HandleCharacterDeselected;
            ActionsManager.Instance.OnApresentadorSelected -= HandleApresentadorSelected;
            ActionsManager.Instance.OnStageEntityDefeated -= HandleStageEntityDefeated;
            ActionsManager.Instance.OnCombatWon -= HandleCombatEnded;
            ActionsManager.Instance.OnCombatLost -= HandleCombatEnded;
            ActionsManager.Instance.OnPauseToggle -= HandlePauseToggle;
            ActionsManager.Instance.OnCombatSpeedChanged -= ApplyBlendTime;
        }
        #region Static API
        public static IEnumerator FocusEntity(StageEntityController entity)
        {
            if (Instance == null || entity == null) yield break;

            Instance._enemyFocus.SetTarget(entity.transform, true);
            Instance._enemyCamera.PreviousStateIsValid = false;
            Instance._enemyCamera.Priority = EnemyLivePriority;
            Instance._isEnemyViewLive = true;

            yield return new WaitForSeconds(Instance._enemyFocusSettleTime / CombatSpeed);
        }
        public static IEnumerator ReturnToPlayerView()
        {
            if (Instance == null || !Instance._isEnemyViewLive) yield break;

            Instance._enemyCamera.Priority = StandbyPriority;
            Instance._enemyFocus.SetTarget(null);
            Instance._isEnemyViewLive = false;

            yield return Instance.WaitForBlend();

            yield return new WaitForSeconds(Instance._returnSettleTime / CombatSpeed);
        }

        #endregion

        private IEnumerator WaitForBlend()
        {
            if (_brain == null) yield break;

            float elapsed = 0f;
            float timeout = (_baseBlendTime / CombatSpeed) + 1f;

            while (_brain.IsBlending && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        #region Handlers

        private void HandleCharacterSelected(CharacterController character)
        {
            Focus(character != null ? character.transform : null);
        }
        private void HandleCharacterDeselected()
        {
            Focus(null);
        }
        private void HandleApresentadorSelected()
        {
            ApresentadorController apresentador = CombatManager.Apresentador;
            bool shouldFocus = _focusApresentadorOnSelect && apresentador != null;

            Focus(shouldFocus ? apresentador.transform : null);
        }

        private void Focus(Transform target)
        {
            if (_isEnemyViewLive) return;

            _playerFocus.SetTarget(target);
        }
        private void HandleStageEntityDefeated(StageEntityController entity)
        {
            if (_playerFocus.Target == entity.transform) _playerFocus.SetTarget(null);
            if (_enemyFocus.Target == entity.transform) _enemyFocus.SetTarget(null);
        }
        private void HandleCombatEnded()
        {
            _enemyCamera.Priority = StandbyPriority;
            _enemyFocus.SetTarget(null);
            _playerFocus.SetTarget(null);
            _isEnemyViewLive = false;
        }
        private void HandlePauseToggle(bool isPaused)
        {
            _playerAxisController.enabled = !isPaused;
        }
        private void ApplyBlendTime()
        {
            if (_brain == null) return;

            _brain.DefaultBlend.Time = _baseBlendTime / CombatSpeed;
        }
        #endregion
    }
}
