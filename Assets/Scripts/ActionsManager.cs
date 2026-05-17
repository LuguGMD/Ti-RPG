using Lugu.Singleton;
using RPG.Combat;
using RPG.Combat.Actions;
using RPG.Combat.Preview;
using RPG.Level;
using RPG.Level.Challenge;
using RPG.Management.Minigames;
using System;
using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    public class ActionsManager : Singleton<ActionsManager>
    {
        #region Combat

        public Action OnCombatStart;

        public Action OnMapChanged;
        public Action OnRotationAnimationStarted;
        public Action OnRotationAnimationEnded;

        public Action OnCombatWon;
        public Action OnCombatLost;

        public Action OnCombatSpeedChanged;
        public Action OnApresentadorDamageTaken;

        public Action OnApresentadorActionCompleted;
        public Action OnApresentadorActionCanceled;

        public Action<StageEntityController> OnStageEntityCreated;
        public Action<StageEntityController> OnStageEntityDefeated;

        public Action<CharacterController> OnCharacterDamageTaken;
        public Action<CharacterController> OnCharacterCreated;
        public Action<CharacterController> OnCharacterDefeated;

        public Action<EnemyController> OnEnemyDefeated;

        public Action<Vector2Int> OnSpotlightPositionChanged;

        #region Effect Triggers

        public Action OnActionStart;
        public Action OnActionEnd;
        public Action OnPatternEnd;
        public Action OnTileStepBefore;
        public Action OnTileStepAfter;

        #endregion

        #region Tile Interaction

        public Action<PreviewTileInfo> OnActionTileSelected;
        public Action<Vector2Int> OnPreviewTileSelected;
        public Action<Vector2Int> OnTileHovered;

        #endregion

        #region Selection

        public Action<EntityController> OnEntitySelected;
        public Action<EntityController> OnEntityHovered;

        public Action<CharacterController> OnCharacterClicked;

        public Action<CharacterController> OnCharacterSelected;
        public Action OnCharacterDeselected;

        public Action OnApresentadorSelected;

        #endregion

        #region Turns

        public Action OnTurnPassed;

        public Action OnPlayerTurnStarted;
        public Action OnPlayerTurnEnded;

        public Action OnEnemyTurnStarted;
        public Action OnEnemyTurnEnded;

        #endregion

        #region Challenge

        public Action<ChallengeHandler> OnChallengeProgressChanged;
        public Action<ChallengeHandler> OnChallengeCompleted;

        #endregion

        #endregion

        #region Management

        #region Minigames

        public Action<CharacterScriptable> OnCharacterMinigameSelected;

        public Action OnMinigameStart;
        public Action OnMinigameEnd;

        public Action OnMinigameMiss;
        public Action OnMinigameHit;
        public Action OnMinigamePerfectHit;

        public Action OnMinigameValuesUpdated;
        public Action<MinigameChallenge> OnMinigameChallengeUpdated;
        public Action OnMinigameChallengeCompleted;

        public Action<CharacterScriptable> OnCharacterMotivated;

        #endregion

        #region Level

        public Action<LevelScriptable> OnLevelSelected;

        #endregion

        #region Upgrades

        public Action OnUpgradePanelOpen;
        public Action OnUpgradePanelClose;

        #endregion

        #endregion

        #region Dialogue

        public Action OnDialogueStart;
        public Action OnDialogueEnd;

        #endregion
    }
}