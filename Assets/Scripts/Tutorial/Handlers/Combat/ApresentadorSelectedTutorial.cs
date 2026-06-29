using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public class ApresentadorSelectedTutorial : TutorialHandler
    {
        public override void SubscribeCallbacks()
        {
            ActionsManager.Instance.OnApresentadorUIOpen += Show;
        }

        public override void UnsubscribeCallbacks()
        {
            ActionsManager.Instance.OnApresentadorUIOpen -= Show;
        }
    }
}
