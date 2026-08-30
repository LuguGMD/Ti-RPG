using RPG.Save;
using UnityEngine;

namespace RPG.UI
{
    public class ResetSaveButton : PlayButton
    {
        protected override void OnClick()
        {
            SaveManager.ResetSave();

            base.OnClick();
        }
    }
}
