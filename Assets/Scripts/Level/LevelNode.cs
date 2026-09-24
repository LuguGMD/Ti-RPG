using RPG.UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level
{
    public class LevelNode : UIButtonHandler
    {
        [Header("Level Data")]
        [SerializeField] 
        private LevelScriptable _levelData;

        #region Properties
        
        public LevelScriptable LevelData
        {
            get { return _levelData; }
        }

        #endregion

        public void SetEnabled()
        {
            if(_button == null) _button = GetComponent<Button>();
            _button.interactable = true;
        }

        public void SetDisabled()
        {
            if(_button == null) _button = GetComponent<Button>();
            _button.interactable = false;
        }   

        protected override void OnClick()
        {
            ActionsManager.Instance.OnLevelSelected?.Invoke(_levelData);
        }
    }
}
