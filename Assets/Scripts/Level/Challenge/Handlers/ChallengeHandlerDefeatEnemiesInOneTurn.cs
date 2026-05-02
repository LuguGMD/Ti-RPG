using RPG.Combat;
using System;
using UnityEngine;

namespace RPG.Level.Challenge.Handlers
{
    public class ChallengeHandlerDefeatEnemiesInOneTurn : ChallengeHandler
    {
        public override void SubscribeCallbacks()
        {
            ActionsManager.Instance.OnEnemyDefeated += OnEnemyDefeated;
            ActionsManager.Instance.OnTurnPassed += ResetProgress;
        }

        public override void UnsubscribeCallbacks()
        {
            ActionsManager.Instance.OnEnemyDefeated -= OnEnemyDefeated;
            ActionsManager.Instance.OnTurnPassed -= ResetProgress;
        }

        private void OnEnemyDefeated(EnemyController enemy)
        {
            AddProgress(1);
        }
    }
}
