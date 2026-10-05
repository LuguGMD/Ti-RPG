using DG.Tweening;
using RPG.UI.Tooltip;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterMotivationSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _characterIcon;
        [SerializeField] private Button _characterButton;
        [SerializeField] private TooltipTrigger _tooltipTrigger;
        private CharacterController _characterController;

        #region Properties

        public Slider Slider { get { return _slider; } }

        #endregion


        private void Awake()
        {
            _characterButton.onClick.AddListener(() => { OnSelect(_characterController); });
        }

        public void SetInfo(CharacterController characterController)
        {
            CharacterScriptable characterInfo = characterController.CharacterInfo;
            _characterController = characterController;
            _characterIcon.sprite = characterController.HasActed ? characterInfo.UsedIcon : characterInfo.Icon;
            _tooltipTrigger.header = characterInfo.EntityName;
            _tooltipTrigger.content = "";

            if (characterController.HasActed)
            {
                _characterIcon.rectTransform.DOScale(Vector3.one * 0.8f, 0.2f);
            }
            else
            {
                _characterIcon.rectTransform.DOScale(Vector3.one, 0.2f);
            }
        }

        public void OnSelect(CharacterController characterController)
        {
            if (!CombatManager.HasCombatStarted) return;
            if (CombatManager.IsActionInProgress) return;
            if (characterController == null) return;

            ActionsManager.Instance.OnEntitySelected?.Invoke(characterController);
            ActionsManager.Instance.OnCharacterClicked?.Invoke(characterController);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ActionsManager.Instance.OnCharacterHoverEnter?.Invoke(_characterController);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ActionsManager.Instance.OnCharacterHoverExit?.Invoke(_characterController);

        }
    }
}
