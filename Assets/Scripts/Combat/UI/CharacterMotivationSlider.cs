using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterMotivationSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _characterNameText;

        #region Properties

        public Slider Slider { get { return _slider; } }

        #endregion

        public void SetName(string name)
        {
            _characterNameText.text = name;
        }
    }
}
