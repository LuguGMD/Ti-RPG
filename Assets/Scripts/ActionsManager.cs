using Lugu.Singleton;
using RPG.Combat;
using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System;
using UnityEngine;

namespace RPG
{
    public class ActionsManager : Singleton<ActionsManager>
    {
        public Action OnMapChanged;

        #region Combat

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

        public Action<EntityController> OnEntitySelected;
        public Action<EntityController> OnEntityHovered;

        #endregion
    }
}