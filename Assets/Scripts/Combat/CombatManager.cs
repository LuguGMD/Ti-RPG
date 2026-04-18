using Lugu.Singleton;
using RPG.Combat.Preview;
using RPG.Combat.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

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

        private bool _isActionInProgress = false;

        public static readonly Dictionary<CombatType, CombatType> TypeChart = new Dictionary<CombatType, CombatType>()
        {
            { CombatType.Magic, CombatType.Anger },
            { CombatType.Strength, CombatType.Fear },
            { CombatType.Jokes, CombatType.Sadness },
            { CombatType.Fear, CombatType.Magic },
            { CombatType.Sadness, CombatType.Strength },
            { CombatType.Anger, CombatType.Jokes },
        };
        private const int MAX_CHARACTERS_COUNT = 3;


        #region Properties

        public static PreviewTile PreviewTilePrefab
        {
            get { return Instance._previewTilePrefab; }
        }
        public BattleTurnState CurrentTurnState { get { return _currentTurnState; } }
        public int TurnCount { get { return _turnCount; } }

        #endregion

        private void Start()
        {
            _remainingEnemies = GameObject.FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID).ToList();
            InitializeBattle();
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
            return TypeChart[user] == target;
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
            _currentTurnState = BattleTurnState.PlayerTurn;
            _turnCount = 1;
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
                _turnCount++;
                Debug.Log($"Turno {_turnCount}: Enemy -> Player");
                ActionsManager.Instance.OnPlayerTurnStarted?.Invoke();

                StartPlayerTurn();
            }
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
            if(_usedCharacters.Count >= MAX_CHARACTERS_COUNT)
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

        #endregion

        #region Metodos Enemy
        
        private IEnumerator EnemyTurnCoroutine()
        {
            for(int i = 0; i< _remainingEnemies.Count; i++)
            {
                yield return _remainingEnemies[i].UseAction(0, 0, 1, false);
                yield return new WaitForSeconds(0.5f);
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

        #endregion

        #endregion
    }
}