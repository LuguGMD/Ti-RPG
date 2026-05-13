using Lugu.Singleton;
using Lugu.UI;
using System;
using UnityEngine;

namespace RPG.Combat.UI
{
    public class CombatUIManager : SingletonMono<CombatUIManager>
    {
        private PanelsController _panelsController;
        [SerializeField] private int _defaultPanelIndex;

        #region Properties

        public static int DefaultPanelIndex { get { return Instance._defaultPanelIndex; } }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            if(Instance == this)
            {
                _panelsController = GetComponent<PanelsController>();
            }
        }

        public void ChangePanel(GameObject panel)
        {
            int index = _panelsController.panelsList.IndexOf(panel);
            _panelsController.ChangePanel(index);
        }

        public void ChangePanel(int panelIndex)
        {
            _panelsController.ChangePanel(panelIndex);
        }

        public void DisablePanel(GameObject panel)
        {
            if (!CombatManager.HasCombatStarted) return;

            if (panel == _panelsController.panelsList[_defaultPanelIndex])
            {
                int index = _panelsController.panelsList.IndexOf(panel);
                _panelsController.ClosePanel(index);
            }
            else
            {
                _panelsController.ChangePanel(_defaultPanelIndex);
            }
            
        }
    }
}
