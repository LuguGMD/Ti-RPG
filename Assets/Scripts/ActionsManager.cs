using Lugu.Singleton;
using RPG.Combat;
using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System;
using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    public class ActionsManager : Singleton<ActionsManager>
    {
        #region Combat

        public Action OnMapChanged;
        public Action OnRotationAnimationStarted;
        public Action OnRotationAnimationEnded;

        public Action OnCombatWon;
        public Action OnCombatLost;

        public Action OnCombatSpeedChanged;
        public Action OnApresentadorDamageTaken;

        public Action OnApresentadorActionCompleted;
        public Action OnApresentadorActionCanceled;

        public Action<CharacterController> OnCharacterDamageTaken;
        public Action<CharacterController> OnCharacterCreated;
        public Action<CharacterController> OnCharacterDefeated;

        #region Effect Triggers

        public Action OnActionStart;
        public Action OnActionEnd;
        public Action OnPatternEnd;
        public Action OnTileStepBefore;
        public Action OnTileStepAfter;

        #endregion

        #region Tile Interaction

        public Action<PreviewTileInfo> OnActionTileSelected;

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

        #endregion

        #endregion
    }
}