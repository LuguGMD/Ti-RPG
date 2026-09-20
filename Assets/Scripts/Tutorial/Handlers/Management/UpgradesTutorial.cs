using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public class UpgradesTutorial : TutorialHandler
    {
        private void Start()
        {
            if(GameManager.Coins >= 1)
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
