using RPG.Save;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TutorialResetButton : UIButtonHandler
    {
        protected override void OnClick()
        {
            GameManager.ResetTutorials();
            SaveManager.Instance.SaveAll();
        }
    }
}
