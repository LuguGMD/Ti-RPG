using UnityEngine;

namespace RPG.Level
{
    public class LevelSelectUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform _mainPanel;

        private static bool _isActive = false;

        #region Properties

        public static bool IsActive { get { return _isActive; } }

        #endregion

        private void OnEnable()
        {
            ActionsManager.Instance.OnLevelSelected += OnLevelSelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnLevelSelected -= OnLevelSelected;
        }

        private void OnLevelSelected(LevelScriptable selectedLevel)
        {
            _mainPanel.gameObject.SetActive(true);
            _isActive = true;
        }

        public void ClosePanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _isActive = false;
        }
    }
}
