using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public class FirstCombatTutorial : TutorialHandler
    {
        private void Start()
        {
            Invoke(nameof(Show), 2f);
        }

        public override void SubscribeCallbacks()
        {

        }

        public override void UnsubscribeCallbacks()
        {

        }
    }
}
