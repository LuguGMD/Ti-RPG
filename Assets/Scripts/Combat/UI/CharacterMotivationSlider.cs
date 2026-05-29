using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterMotivationSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _characterIcon;

        #region Properties

        public Slider Slider { get { return _slider; } }

        #endregion


        public void SetInfo(CharacterController characterController)
        {
            _characterIcon.sprite = characterController.HasActed? characterController.CharacterInfo.UsedIcon : characterController.CharacterInfo.Icon;

            if(characterController.HasActed)
            {
                _characterIcon.rectTransform.DOScale(Vector3.one * 0.8f, 0.2f);
            }
            else
            {
                _characterIcon.rectTransform.DOScale(Vector3.one, 0.2f);
            }
        }
    }
}
