using RPG.Save;
using UnityEngine;

namespace RPG.UI
{
    public class PlayButton : UIButtonHandler
    {
        protected override void OnClick()
        {
            SaveManager.Instance.LoadAll();
            GameManager.ChangeScene(ScenesEnum.WorldMap);
        }
    }
}
