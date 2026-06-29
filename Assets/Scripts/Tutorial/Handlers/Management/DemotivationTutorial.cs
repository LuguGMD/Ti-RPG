using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public class DemotivationTutorial : TutorialHandler
    {
        private void Start()
        {
            if(GameManager.DefeatedCharacters.Count > 0)
            {
                Show();
            }
        }

        public override void SubscribeCallbacks()
        {
            
        }

        public override void UnsubscribeCallbacks()
        {
            
        }
    }
}
