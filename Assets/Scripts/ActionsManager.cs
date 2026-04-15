using Lugu.Singleton;
using RPG.Combat.Actions;
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

        #endregion
    }
}