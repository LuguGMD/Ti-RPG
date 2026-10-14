using RPG.Save;
using UnityEngine;

namespace RPG.UI
{
    public class PlayButton : UIButtonHandler
    {
        private void Start()
        {
            if (!SaveManager.HasSaveData())
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
