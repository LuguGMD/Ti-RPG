using DG.Tweening;
using RPG.UI.Tooltip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterMotivationSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _characterIcon;
        [SerializeField] private Button _characterButton;
        [SerializeField] private TooltipTrigger _tooltipTrigger;

        #region Properties

        public Slider Slider { get { return _slider; } }

        #endregion


        public void SetInfo(CharacterController characterController)
        {
            CharacterScriptable characterInfo = characterController.CharacterInfo;
            _characterIcon.sprite = characterController.HasActed ? characterInfo.UsedIcon : characterInfo.Icon;
            _tooltipTrigger.header = characterInfo.EntityName;
            _tooltipTrigger.content = "";

            _characterButton.onClick.AddListener(() => { OnSelect(characterController); });

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
            ActionsManager.Instance.OnEntitySelected?.Invoke(characterController);
            ActionsManager.Instance.OnCharacterClicked?.Invoke(characterController);
        }
    }
}
