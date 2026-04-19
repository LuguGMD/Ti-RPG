using RPG.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.UI
{
    public class ExitGameButton : UIButtonHandler
    {
        protected override void OnClick()
        {

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        }
    }
}
