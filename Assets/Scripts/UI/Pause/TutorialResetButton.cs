using RPG.Save;
using RPG.Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TutorialResetButton : UIButtonHandler
    {
        protected override void OnClick()
        {
            TutorialManager.Instance.ResetTutorials();
        }
    }
}
