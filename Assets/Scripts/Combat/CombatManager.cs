using Lugu.Singleton;
using RPG.Combat.Preview;
using RPG.Combat.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using RPG.Combat.Wave;
using RPG.Combat.Grid;
using RPG.Combat.UI;
using Unity.Cinemachine;
using RPG.Save;
using RPG.Management.Progression;

namespace RPG.Combat
{
    public class CombatManager : SingletonMono<CombatManager>
    {
        [Header("Enemies")]
        [SerializeField] private EnemySpawnWarning _enemySpawnWarningPrefab;
        [Header("Tiles")]
        [SerializeField] private ActionPreviewTile _previewTilePrefab;
        [SerializeField] private PreviewTileGroup _characterPreviewGroups;
        [SerializeField] private PreviewTileGroup _enemyPreviewGroups;
        private CinemachineImpulseSource _impulseSource;

        private CombatTurnStateEnum _currentTurnState;
        private int _turnCount;
        private bool _hasCombatStarted = false;
        private bool _hasCombatEnded = false;

        private CharacterController _selectedCharacter;
        private List<CharacterController> _usedCharacters = new List<CharacterController>();
        private bool _canSelectCharacter = true;

        private List<EnemyController> _remainingEnemies = new List<EnemyController>();
        private List<CharacterController> _remainingCharacters = new List<CharacterController>();
        private ApresentadorController _apresentador;

        private bool _isActionInProgress = false;
        private float _combatSpeed = 1;

        private HashSet<string> _combatUpgrades = new HashSet<string>();

        #region Properties

        public static EnemySpawnWarning EnemySpawnWarningPrefab { get { return Instance._enemySpawnWarningPrefab; } }
        public static ActionPreviewTile PreviewTilePrefab
        {
            get { return Instance._previewTilePrefab; }
        }
        public static PreviewTileGroup CharacterPreviewGroups { get { return Instance._characterPreviewGroups; } }
        public static PreviewTileGroup EnemyPreviewGroups { get { return Instance._enemyPreviewGroups; } }
        public static CombatTurnStateEnum CurrentTurnState { get { return Instance._currentTurnState; } }
        public static int TurnCount { get { return Instance._turnCount; } }
        public static float CombatSpeed { get { return Instance._combatSpeed; } }
        public static ApresentadorController Apresentador { get { return Instance._apresentador; } }
        public static bool HasCombatStarted { get { return Instance._hasCombatStarted; } }
        public static List<CharacterController> RemainingCharacters { get { return Instance._remainingCharacters; } }
        public static bool IsActionInProgress { get { return Instance._isActionInProgress; } }
        public static HashSet<string> CombatUpgrades { get { return Instance._combatUpgrades; }  }

        #endregion

        protected override void Awake()
        {
            base.Awake();


            if (Instance == this)
            {
                GetCombatUpgrades();
                _impulseSource = GetComponent<CinemachineImpulseSource>();
            }
        }

        private void Start()
        {
            if (Instance != this) return;

            _apresentador = GameObject.FindAnyObjectByType<ApresentadorController>(FindObjectsInactive.Include);
            _apresentador.gameObject.SetActive(true);
            ActionsManager.Instance.OnPreviewTileSelected += PlaceCharacter;
        }

        private void OnEnable()
        {
            //if (Instance != null && Instance != this) return;

            ActionsManager.Instance.OnCombatStart += InitializeBattle;
            ActionsManager.Instance.OnCharacterClicked += OnCharacterClicked;
            ActionsManager.Instance.OnApresentadorSelected += OnApresentadorSelected;
            ActionsManager.Instance.OnActionTileSelected += OnCombatActionSelected;
            ActionsManager.Instance.OnApresentadorActionCompleted += CheckEndPlayerTurn;
            ActionsManager.Instance.OnPlayerTurnEnded += EndPlayerTurn;
            ActionsManager.Instance.OnCharacterDamageTaken += CharacterDamageTaken;
            ActionsManager.Instance.OnApresentadorDamageTaken += ApresentadorDamageTaken;
        }

        private void OnDisable()
        {
            //if (Instance != null && Instance != this) return;

            ActionsManager.Instance.OnCombatStart -= InitializeBattle;
            ActionsManager.Instance.OnCharacterClicked -= OnCharacterClicked;
            ActionsManager.Instance.OnApresentadorSelected -= OnApresentadorSelected;
            ActionsManager.Instance.OnActionTileSelected -= OnCombatActionSelected;
            ActionsManager.Instance.OnApresentadorActionCompleted -= CheckEndPlayerTurn;
            ActionsManager.Instance.OnPlayerTurnEnded -= EndPlayerTurn;
            ActionsManager.Instance.OnCharacterDamageTaken -= CharacterDamageTaken;
            ActionsManager.Instance.OnApresentadorDamageTaken -= ApresentadorDamageTaken;
            ActionsManager.Instance.OnPreviewTileSelected -= PlaceCharacter;
        }

        #region Preparation

        private void GetCombatUpgrades()
        {
            UpgradeGraphData data = SaveManager.SaveData.UpgradeGraphData;

            if (data == null) return;

            foreach(string upgradeKey in data.PurchasedUpgradeIDs)
            {
                _combatUpgrades.Add(upgradeKey);
            }
        }

        private void PlaceCharacter(Vector2Int position)
        {
            if (MapManager.Map.GetTile(position).IsOccupied)
            {
                //TO DO mostrar erro ao jogador
                Debug.Log("Tile ocupado");
            }
            else if (position.y >= Map.Rows - 1)
            {
                //TO DO mostrar erro ao jogador
                Debug.Log("Tile Invalido");
            }
            else
            {
                CharacterScriptable characterInfo = GameManager.CurrentParty[_remainingCharacters.Count];
                CharacterSpawnInfo characterSpawnInfo = new CharacterSpawnInfo(characterInfo, position);
                CombatFactory.InstantiateCharacter(characterSpawnInfo);
            }

        }

        #endregion

        #region Control


        public static bool CanTarget(EntityScriptable user, EntityScriptable target, Effect effect)
        {
            if (user == target)
            {
                return effect.CanTargetSelf;
            }

            return effect.TargetList.Contains(target.Team);
        }

        #endregion

        #region Feedback

        private void CharacterDamageTaken(CharacterController character)
        {
            CameraShake(0.1f);
        }

        private void ApresentadorDamageTaken()
        {
            CameraShake(0.25f);
        }

        public void CameraShake(float force)
        {
            _impulseSource.GenerateImpulse(force);
        }


        #endregion

        private void OnCharacterClicked(CharacterController selectedCharacter)
        {
            if (_hasCombatEnded || !_hasCombatStarted)
            {

            }
            else if (_usedCharacters.Contains(selectedCharacter))
            {

            }
            else if (!_canSelectCharacter)
            {

            }
            else
            {
                DeselectCharacter();

                _selectedCharacter = selectedCharacter;
                ActionsManager.Instance.OnCharacterSelected?.Invoke(_selectedCharacter);

                //TO DO passar para quando acao for selecionada
                _selectedCharacter.Preview.ShowPreview();
            }
        }

        private void OnApresentadorSelected()
        {
            DeselectCharacter();
        }

        private void OnCombatActionSelected(PreviewTileInfo previewTileInfo)
        {
            if (_selectedCharacter == null || _hasCombatEnded) return;

            StartCoroutine(PlayerActionCoroutine(previewTileInfo));
        }

        #region Metodos Combate

        private void InitializeBattle()
        {
            ActionsManager.Instance.OnPreviewTileSelected -= PlaceCharacter;
            CombatUIManager.Instance.ChangePanel(CombatUIManager.DefaultPanelIndex);
            _turnCount = 0;
            _currentTurnState = CombatTurnStateEnum.PlayerTurn;
            _hasCombatStarted = true;
            _hasCombatEnded = false;
            StartPlayerTurn();
            SwitchTurn();
        }

        private void SwitchTurn()
        {
            if (_currentTurnState == CombatTurnStateEnum.PlayerTurn)
            {
                _currentTurnState = CombatTurnStateEnum.EnemyTurn;
                ActionsManager.Instance.OnEnemyTurnStarted?.Invoke();

                StartEnemyTurn();
            }
            else if (_currentTurnState == CombatTurnStateEnum.EnemyTurn)
            {
                ActionsManager.Instance.OnEnemyTurnEnded?.Invoke();
                _currentTurnState = CombatTurnStateEnum.PlayerTurn;
                PassTurn();
                ActionsManager.Instance.OnPlayerTurnStarted?.Invoke();

                StartPlayerTurn();
            }
        }

        private void PassTurn()
        {
            _turnCount++;
            ActionsManager.Instance.OnTurnPassed?.Invoke();
        }

        public void SetCombatSpeed(int speedTier)
        {
            _combatSpeed = CombatConstants.CombatSpeedTiers[speedTier];
            ActionsManager.Instance.OnCombatSpeedChanged?.Invoke();
        }

        public void PassCombatSpeedTier()
        {
            int currentSpeedTier = Array.IndexOf(CombatConstants.CombatSpeedTiers, _combatSpeed);
            currentSpeedTier++;

            currentSpeedTier %= CombatConstants.CombatSpeedTiers.Length;
            SetCombatSpeed(currentSpeedTier);
        }

        #region Metodos Player

        private IEnumerator PlayerActionCoroutine(PreviewTileInfo previewTileInfo)
        {
            _usedCharacters.Add(_selectedCharacter);

            _canSelectCharacter = false;
            _isActionInProgress = true;
            _selectedCharacter.Preview.HidePreview();
            HideAllEnemiesPreviews();

            yield return _selectedCharacter.UseSelectedAction(previewTileInfo);

            ShowAllEnemiesPreviews();
            DeselectCharacter();
            _canSelectCharacter = true;
            _isActionInProgress = false;


            CheckEndPlayerTurn();

        }

        private void CheckEndPlayerTurn()
        {
            if (_usedCharacters.Count >= _remainingCharacters.Count && _apresentador.HasActed)
            {
                EndPlayerTurn();
            }
        }

        private void EndPlayerTurn()
        {
            if (!_isActionInProgress && _currentTurnState == CombatTurnStateEnum.PlayerTurn)
            {
                _apresentador.CompleteAction();
                SwitchTurn();
            }
        }

        private void DeselectCharacter()
        {
            _selectedCharacter?.Preview.HidePreview();
            ActionsManager.Instance.OnCharacterDeselected?.Invoke();

            _selectedCharacter = null;

        }

        private void StartPlayerTurn()
        {
            ShowAllEnemiesPreviews();

            _apresentador.ResetAction();
            _usedCharacters.Clear();
            _canSelectCharacter = true;
        }

        public void AddCharacter(CharacterController character)
        {
            if (!_remainingCharacters.Contains(character))
            {
                _remainingCharacters.Add(character);
            }

            if (_remainingCharacters.Count >= CombatConstants.MAX_CHARACTERS_COUNT)
            {
                ActionsManager.Instance.OnCombatStart?.Invoke();
            }
        }

        public void RemoveCharacter(CharacterController character)
        {
            if (_remainingCharacters.Contains(character))
            {
                _remainingCharacters.Remove(character);
            }

            CheckPlayerLost();
        }

        private void CheckPlayerLost()
        {
            if (_remainingCharacters.Count <= 0)
            {
                ActionsManager.Instance.OnCombatLost?.Invoke();
                _hasCombatEnded = true;

                //TO DO remover depois
                GameManager.ChangeScene(ScenesEnum.Lose);
            }
        }

        private void CheckPlayerWon()
        {
            if (WaveManager.AreAllWavesSpawned && _remainingEnemies.Count == 0)
            {
                ActionsManager.Instance.OnCombatWon?.Invoke();
                _hasCombatEnded = true;

                if(!GameManager.CompletedLevels.Contains(GameManager.SelectedLevel.LevelKey))
                    GameManager.Instance.AddCoins(GameManager.SelectedLevel.CoinsReward);

                //TO DO remover depois
                GameManager.ChangeScene(ScenesEnum.Win);
            }
        }

        #endregion

        #region Metodos Enemy

        private IEnumerator EnemyTurnCoroutine()
        {
            for (int i = 0; i < _remainingEnemies.Count; i++)
            {
                if (!_hasCombatEnded)
                {
                    yield return _remainingEnemies[i].UsePreparedAction();
                    yield return new WaitForSeconds(0.1f / _combatSpeed);
                }
            }

            SwitchTurn();
        }

        private void StartEnemyTurn()
        {
            DeselectCharacter();
            _canSelectCharacter = false;

            HideAllEnemiesPreviews();

            StartCoroutine(EnemyTurnCoroutine());
        }

        private void ShowAllEnemiesPreviews()
        {
            for (int i = 0; i < _remainingEnemies.Count; i++)
            {
                _remainingEnemies[i].PrepareAction();
                _remainingEnemies[i].Preview.ShowPreview();
            }
        }

        private void HideAllEnemiesPreviews()
        {
            for (int i = 0; i < _remainingEnemies.Count; i++)
            {
                _remainingEnemies[i].Preview.HidePreview();
            }
        }

        public void AddEnemy(EnemyController enemy)
        {
            if (!_remainingEnemies.Contains(enemy))
            {
                _remainingEnemies.Add(enemy);
            }
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            if (_remainingEnemies.Contains(enemy))
            {
                _remainingEnemies.Remove(enemy);
            }

            CheckPlayerWon();
        }

        #endregion

        #endregion

        public static bool HasUpgrade(UpgradeConstants.UpgradeKey upgradeKey)
        {
            return CombatUpgrades.Contains(UpgradeConstants.UpgradeKeys[upgradeKey]);
        }
    }
}