using Lugu.UI;
using UnityEngine;

namespace RPG.UI
{
    public class ChangePanelButton : UIButtonHandler
    {
        [SerializeField] private PanelsController _panelsController;
        [SerializeField] private int _panelIndex;

        protected override void OnClick()
        {
            _panelsController.ChangePanel(_panelIndex);
        }
    }
}
