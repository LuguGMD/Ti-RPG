using UnityEngine;

namespace RPG.UI
{
    public class ChangeSceneButton : UIButtonHandler
    {
        [SerializeField] private ScenesEnum _scene;

        protected override void OnClick()
        {
            GameManager.ChangeScene(_scene);
        }
    }
}
