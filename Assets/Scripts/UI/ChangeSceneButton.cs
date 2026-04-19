using UnityEngine;

namespace RPG.UI
{
    public class ChangeSceneButton : UIButtonHandler
    {
        [SerializeField] private int _sceneBuildIndex;

        protected override void OnClick()
        {
            GameManager.ChangeScene(_sceneBuildIndex);
        }
    }
}
