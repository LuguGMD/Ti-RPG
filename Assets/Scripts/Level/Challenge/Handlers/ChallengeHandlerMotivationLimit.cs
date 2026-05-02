using RPG.Combat;
using RPG.Level.Challenge;
using UnityEngine;

namespace RPG.Level.Challenge.Handlers
{
    public class ChallengeHandlerMotivationLimit : ChallengeHandler
    {
        public override void SubscribeCallbacks()
        {
            ActionsManager.Instance.OnCombatWon += OnCombatWon;
        }

        public override void UnsubscribeCallbacks()
        {
            ActionsManager.Instance.OnCombatWon -= OnCombatWon;
        }

        private void OnCombatWon()
        {
            SetProgress(CombatManager.Apresentador.CurrentMotivation);
        }
    }
}
