using Lugu.Singleton;
using RPG.Combat.Preview;
using RPG.Combat.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using RPG.Combat.Wave;

namespace RPG.Combat
{
    public class CombatManager : SingletonMono<CombatManager>
    {
        [SerializeField] private PreviewTile _previewTilePrefab;

        private BattleTurnState _currentTurnState;
        private int _turnCount;
        private bool _isBattleOver;

        private CharacterController _selectedCharacter;
        private List<CharacterController> _usedCharacters = new List<CharacterController>();
        private bool _canSelectCharacter = true;

        private List<EnemyController> _remainingEnemies = new List<EnemyController>();
        private List<CharacterController> _remainingCharacters = new List<CharacterController>();
        private ApresentadorController _apresentador;

        private bool _isActionInProgress = false;
        private float _combatSpeed = 1;
        
        
        

        #region Properties

        public static PreviewTile PreviewTilePrefab
        {
            get { return Instance._previewTilePrefab; }
        }
        public static BattleTurnState CurrentTurnState { get { return Instance._currentTurnState; } }
        public static int TurnCount { get { return Instance._turnCount; } }
        public static float CombatSpeed {  get { return Instance._combatSpeed; } }
        public static ApresentadorController Apresentador { get { return Instance._apresentador; } }

        #endregion

        private void Start()
        {
            _apresentador = GameObject.FindAnyObjectByType<ApresentadorController>(FindObjectsInactive.Include);

            //TO DO adicionar escolha da posicao dos personagens antes do comeco do combate
            Invoke(nameof(InitializeBattle),0.01f);
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterSelected += OnCharacterSelected;
            ActionsManager.Instance.OnActionTileSelected += OnCombatActionSelected;
            ActionsManager.Instance.OnPlayerTurnEnded += EndPlayerTurn;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterSelected -= OnCharacterSelected;
            ActionsManager.Instance.OnActionTileSelected -= OnCombatActionSelected;
            ActionsManager.Instance.OnPlayerTurnEnded -= EndPlayerTurn;
        }

        #region Control

        public static bool IsTargetWeak(CombatType user, CombatType target)
        {
            return CombatConstants.TypeChart[user] == target;
        }

        public static void SubscribeEffectTriggerAction(EffectTrigger effectTrigger, Action action)
        {
            switch (effectTrigger)
            {
                case EffectTrigger.ActionStart:
                    ActionsManager.Instance.OnActionStart += action;
                    break;
                case EffectTrigger.ActionEnd:
                    ActionsManager.Instance.OnActionEnd += action;
                    break;
                case EffectTrigger.PatternEnd:
                    ActionsManager.Instance.OnPatternEnd += action;
                    break;
                case EffectTrigger.BeforeTileStep:
                    ActionsManager.Instance.OnTileStepBefore += action;
                    break;
                case EffectTrigger.AfterTileStep:
                    ActionsManager.Instance.OnTileStepAfter += action;
                    break;
            }
        }
        public static void UnsubscribeEffectTriggerAction()
        {
            ActionsManager.Instance.OnActionStart = null;
            ActionsManager.Instance.OnActionEnd = null;
            ActionsManager.Instance.OnPatternEnd = null;
            ActionsManager.Instance.OnTileStepBefore = null;
            ActionsManager.Instance.OnTileStepAfter = null;
        }

        public static bool CanTarget(EntityScriptable user, EntityScriptable target, Effect effect)
        {
            if(user == target)
            {
                return effect.CanTargetSelf;
            }

            return effect.TargetList.Contains(target.Team);
        }

        #endregion

        private void OnCharacterSelected(CharacterController selectedCharacter)
        {
            if (_usedCharacters.Contains(selectedCharacter))
            {

            }
            else if(!_canSelectCharacter)
            {

            }
            else
            {
                DeselectCharacter();

                _selectedCharacter = selectedCharacter;

                _selectedCharacter.Preview.ShowPreview();
            }
        }

        private void OnCombatActionSelected(PreviewTileInfo previewTileInfo)
        {
            if (_selectedCharacter == null) return;

            StartCoroutine(PlayerActionCoroutine(previewTileInfo));
        }

        #region Metodos Combate

        private void InitializeBattle()
        {
            _turnCount = 0;
            _currentTurnState = BattleTurnState.PlayerTurn;
            PassTurn();
            _isBattleOver = false;
            StartPlayerTurn();
        }

        private void SwitchTurn()
        {
            if (_currentTurnState == BattleTurnState.PlayerTurn)
            {
                _currentTurnState = BattleTurnState.EnemyTurn;
                Debug.Log($"Turno {_turnCount}: Player -> Enemy");
                ActionsManager.Instance.OnEnemyTurnStarted?.Invoke();

                StartEnemyTurn();
            }
            else if (_currentTurnState == BattleTurnState.EnemyTurn)
            {
                _currentTurnState = BattleTurnState.PlayerTurn;
                PassTurn();
                Debug.Log($"Turno {_turnCount}: Enemy -> Player");
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
            int patternIndex = previewTileInfo.PatternIndex;
            int repetition = previewTileInfo.PatternRepetitionCount;
            bool isMirrored = previewTileInfo.IsMirrored;

            _usedCharacters.Add(_selectedCharacter);

            _canSelectCharacter = false;
            _isActionInProgress = true;
            _selectedCharacter.Preview.HidePreview();
            HideAllEnemiesPreviews();

            yield return _selectedCharacter.UseAction(0, patternIndex, repetition, isMirrored);

            ShowAllEnemiesPreviews();
            DeselectCharacter();
            _canSelectCharacter = true;
            _isActionInProgress = false;


            CheckEndPlayerTurn();

        }

        private void CheckEndPlayerTurn()
        {
            if(_usedCharacters.Count >= _remainingCharacters.Count)
            {
                EndPlayerTurn();
            }
        }

        private void EndPlayerTurn()
        {
            if(!_isActionInProgress && _currentTurnState == BattleTurnState.PlayerTurn)
            {
                SwitchTurn();
            }
        }

        private void DeselectCharacter()
        {
            _selectedCharacter?.Preview.HidePreview();
            _selectedCharacter = null;
        }

        private void StartPlayerTurn()
        {
            ShowAllEnemiesPreviews();

            _usedCharacters.Clear();
            _canSelectCharacter = true;
        }

        public void AddCharacter(CharacterController character)
        {
            if (!_remainingCharacters.Contains(character))
            {
                _remainingCharacters.Add(character);
            }
        }

        public void RemoveCharacter(CharacterController character)
        {
            if (_remainingCharacters.Contains(character))
            {
                _remainingCharacters.Remove(character);
            }

            CheckPlayerDefeated();
        }

        private void CheckPlayerDefeated()
        {
            if(_remainingCharacters.Count <= 0)
            {
                ActionsManager.Instance.OnCombatLost?.Invoke();
                _isBattleOver = true;
            }
        }

        private void CheckPlayerWon()
        {
            if(WaveManager.AreAllWavesSpawned && _remainingEnemies.Count == 0)
            {
                ActionsManager.Instance.OnCombatWon?.Invoke();
                _isBattleOver = true;
            }
        }

        #endregion

        #region Metodos Enemy

        private IEnumerator EnemyTurnCoroutine()
        {
            for(int i = 0; i< _remainingEnemies.Count; i++)
            {
                yield return _remainingEnemies[i].UseAction(0, 0, 1, false);
                yield return new WaitForSeconds(0.1f / _combatSpeed);
            }

            SwitchTurn();
        }

        private void StartEnemyTurn()
        {
            _canSelectCharacter = false;

            HideAllEnemiesPreviews();

            StartCoroutine(EnemyTurnCoroutine());
        }

        private void ShowAllEnemiesPreviews()
        {
            for (int i = 0; i < _remainingEnemies.Count; i++)
            {
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
    }
}