using RPG.Save;
using UnityEngine;

namespace RPG.UI
{
    public class ResetSaveButton : UIButtonHandler
    {
        protected override void OnClick()
        {
            SaveManager.ResetSave();
        }
    }
}
