using System.Collections.Generic;
using UnityEngine;

namespace Lugu.UI
{
    public class PanelsController : MonoBehaviour
    {
        [SerializeField] private int _startCanvasIndex;
        [SerializeField] private List<GameObject> _panelsList;

        #region Properties

        public List<GameObject> panelsList
        {
            get { return _panelsList; }
            private set { _panelsList = value; }
        }

        #endregion

        private void Awake()
        {
            ChangePanel(_startCanvasIndex);
        }

        public void ChangePanel(int panelIndex)
        {
            for (int i = 0; i < _panelsList.Count; i++)
            {
                _panelsList[i].SetActive(i == panelIndex);
            }
        }

        public void ClosePanel(int panelIndex)
        {
            _panelsList[panelIndex].SetActive(false);
        }

        public void OpenPanel(int panelIndex)
        {
            _panelsList[panelIndex].SetActive(true);
        }
    }
}

