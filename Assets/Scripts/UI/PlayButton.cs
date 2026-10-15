using RPG.Save;
using UnityEngine;

namespace RPG.UI
{
    public class PlayButton : UIButtonHandler
    {
        private void Start()
        {
            ResetSaveButton resetSaveButton = GetComponentInChildren<ResetSaveButton>(true);
            if (resetSaveButton == null && (SaveManager.SaveData == null || SaveManager.SaveData.IsFirstTime))
            {
                gameObject.SetActive(false);
            }
        }

        protected override void OnClick()
        {
            SaveManager.Instance.LoadAll();
            GameManager.ChangeScene(ScenesEnum.Management);
        }
    }
}
