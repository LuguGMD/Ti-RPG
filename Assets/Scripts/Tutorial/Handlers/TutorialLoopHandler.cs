using System.Collections;
using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public abstract class TutorialLoopHandler : TutorialHandler
    {
        protected new string _tutorialKey = "TutorialLoopDefaultKey";

        public abstract IEnumerator TutorialLoop();
        public override void Show()
        {
            StartCoroutine(TutorialLoop());
        }
    }
}
