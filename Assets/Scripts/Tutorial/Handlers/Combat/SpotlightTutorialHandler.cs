using RPG.Combat;
using RPG.Management.Progression;
using UnityEngine;

namespace RPG.Tutorial.Handlers
{
    public class SpotlightTutorialHandler : TutorialHandler
    {
        private void Start()
        {
            if(CombatManager.HasUpgrade(UpgradeConstants.UpgradeKey.Spotlight))
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
