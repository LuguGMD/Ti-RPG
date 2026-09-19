using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Level
{
    public class CharacterOptionButton : MonoBehaviour, IPointerEnterHandler
    {
        private CharacterScriptable _character;
        [SerializeField] private Image _characterIcon;
        private bool _isSelected = false;

        #region Properties

        public CharacterScriptable Character { get { return _character; } }

        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            LevelSelectUIController.Instance.UpdateCharacterInfo(_character);
        }

        public void OnClick()
        {
            if (!_isSelected)
            {
                LevelSelectUIController.Instance.AddPartyMember(_character);
            }
            else
            {
                LevelSelectUIController.Instance.RemovePartyMember(_character);
            }

        }

        public void UpdateInfo(CharacterScriptable character)
        {
            _character = character;
            _characterIcon.sprite = character.Icon;
        }

        public void UpdateVisual(bool isSelected)
        {
            _isSelected = isSelected;
            _characterIcon.sprite = isSelected ? _character.Icon : _character.UsedIcon;
        }
    }
}
