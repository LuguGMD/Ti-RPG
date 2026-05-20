using RPG.Combat;
using RPG.Combat.Challenge;
using UnityEngine;

namespace RPG.Combat.Challenge.Handlers
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
