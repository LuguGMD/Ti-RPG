using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public class FirstCombatStartTutorial : TutorialHandler
    {


        public override void SubscribeCallbacks()
        {
            ActionsManager.Instance.OnCombatStart += Show;
        }

        public override void UnsubscribeCallbacks()
        {
            ActionsManager.Instance.OnCombatStart -= Show;
        }
    }
}
