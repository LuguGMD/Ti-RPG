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
        public Action OnMapChanged;

        #region Combat

        public Action OnCombatWon;
        public Action OnCombatLost;

        public Action OnCombatSpeedChanged;
        public Action OnApresentadorDamageTaken;

        public Action OnApresentadorActionCompleted;
        public Action OnApresentadorActionCanceled;

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

        public Action<CharacterController> OnCharacterSelected;

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