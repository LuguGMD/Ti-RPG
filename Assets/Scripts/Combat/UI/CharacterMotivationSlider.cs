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

        public void SetInfo(CharacterScriptable characterInfo)
        {
            _characterIcon.sprite = characterInfo.Icon;
        }
    }
}
