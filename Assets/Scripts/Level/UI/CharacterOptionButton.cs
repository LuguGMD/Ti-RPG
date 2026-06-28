using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Level
{
    public class CharacterOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CharacterScriptable _character;
        [SerializeField] private Image _characterIcon;
        [SerializeField] private Image _demotivatedIcon;

        private Vector3 originalScale;
        private float scaleMultiplier = 1.1f;

        #region Properties

        public CharacterScriptable Character { get { return _character; } }

        #endregion

        private void Start()
        {
            originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = originalScale * scaleMultiplier;
            LevelSelectUIController.Instance.UpdateCharacterInfo(_character);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = originalScale;
        }

        public void OnClick()
        {
            LevelSelectUIController.Instance.ReplaceCharacter(_character);
        }

        public void UpdateInfo(CharacterScriptable character)
        {
            _character = character;
            _characterIcon.sprite = character.Icon;

            _demotivatedIcon.enabled = GameManager.DefeatedCharacters.Contains(character);
        }
    }
}
