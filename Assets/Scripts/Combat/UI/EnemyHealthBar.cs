using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _currentHealthSlider;
        [SerializeField] private Slider _previousHealthSlider;

        private float _delay = 0.5f;

        public void UpdateHealth(float value)
        {
            _currentHealthSlider.DOValue(value, 0.2f);
            _previousHealthSlider.DOValue(value, 0.2f).SetDelay(_delay);
        }

    }
}
