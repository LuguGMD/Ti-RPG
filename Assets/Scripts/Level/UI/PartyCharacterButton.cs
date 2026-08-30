using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Level
{
    public class PartyCharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CharacterScriptable _character;
        private int _partyIndex;

        private Vector3 originalScale;
        private float scaleMultiplier = 1.25f;

        [SerializeField] private Image _characterIcon;
        [SerializeField] private Image _demotivatedIcon;

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
            LevelSelectUIController.Instance.SelectPartyMember(_partyIndex);
        }

        public void SetIndex(int index)
        {
            _partyIndex = index;
        }

        public void UpdateInfo(CharacterScriptable character)
        {
            _character = character;
            _characterIcon.sprite = character.Icon;
        }
    }
}
