using RPG.Combat.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace RPG.Combat.UI
{
    public class ActionButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private int _actionIndex;
        private CombatAction _action;
        private CharacterController _character;
        private CharacterActionPanelController _panelController;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _entranceDuration = 0.3f;
        [SerializeField] private float _selectDuration = 0.2f;
        [SerializeField] private float _entranceDelay = 0.05f;

        private float _selectScale = 1.3f;

        public void Initialize(int actionIndex, CombatAction action, CharacterController character, CharacterActionPanelController panelController)
        {
            _actionIndex = actionIndex;
            _action = action;
            _character = character;
            _panelController = panelController;

            _button.GetComponent<Image>().color = character.Info.EntityColor;

            _rect.DOScale(Vector3.one, 0.2f);

            _buttonText.text = action.ActionName;
            _button.onClick.AddListener(OnButtonClick);

            _rect.DOKill();
            _rect.localScale = Vector3.zero;

            _rect.DOScale(Vector3.one, _entranceDuration)
                .SetDelay(_actionIndex * _entranceDelay)
                .SetEase(Ease.OutBack);
        }

        private void OnButtonClick()
        {
            if (_panelController != null)
            {
                _panelController.OnActionButtonClicked();
                _panelController.OnActionSelected(_actionIndex);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_panelController != null)
            {
                _panelController.OnActionButtonHovered(_action);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_panelController != null)
            {
                _panelController.OnActionButtonExited();
            }
        }

        public void Select(int actionIndex)
        {
            _rect.DOKill();

            Vector3 targetScale = actionIndex == _actionIndex
                ? Vector3.one * _selectScale
                : Vector3.one;

            _rect.DOScale(targetScale, _selectDuration)
                .SetEase(Ease.OutBack);
        }
    }
}
