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

        private float _selectScale = 1.1f;

        public void Initialize(int actionIndex, CombatAction action, CharacterController character, CharacterActionPanelController panelController)
        {
            _actionIndex = actionIndex;
            _action = action;
            _character = character;
            _panelController = panelController;

            _buttonText.text = action.ActionName;
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (_panelController != null)
            {
                _panelController.OnActionButtonClicked();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_panelController != null)
            {
                _panelController.OnActionSelected(_actionIndex);
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
            if (actionIndex == _actionIndex)
            {
                _rect.DOScale(Vector3.one* _selectScale, 0.2f);
            }
            else
            {
                _rect.DOScale(Vector3.one, 0.2f);
            }
        }
    }
}
