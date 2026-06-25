using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public abstract class TutorialInfiniteHandler : TutorialHandler
    {
        protected new string _tutorialKey = "TutorialInfiniteDefaultKey";

        protected new void Complete()
        {
            _isCompleted = true;
            enabled = false;
        }
    }
}
