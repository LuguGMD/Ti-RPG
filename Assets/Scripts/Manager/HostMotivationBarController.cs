using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class HostMotivationBarController : MonoBehaviour
    {
        [SerializeField] private Slider _motivationSlider;
        [SerializeField] private float _maxMotivation = 100f;
        private float _currentMotivation;

        #region Properties

        public float MaxMotivation { get { return _maxMotivation; } set { _maxMotivation = value; } }
        public float CurrentMotivation { get { return _currentMotivation; } }

        #endregion

        private void OnEnable()
        {
            if (_motivationSlider != null)
            {
                _motivationSlider.interactable = false;
            }
        }

        public void Initialize()
        {
            _currentMotivation = _maxMotivation;

            if (_motivationSlider != null)
            {
                _motivationSlider.maxValue = _maxMotivation;
                _motivationSlider.value = _currentMotivation;
            }

            Debug.Log($"[HostMotivationBarController] Apresentador inicializado com motivação: {_currentMotivation}/{_maxMotivation}");
        }

        public void TakeDamage(float damageValue)
        {
            if (damageValue <= 0)
            {
                Debug.LogWarning("[HostMotivationBarController] Valor de dano deve ser maior que zero.");
                return;
            }

            _currentMotivation -= damageValue;

            if (_currentMotivation < 0)
            {
                _currentMotivation = 0;
            }

            UpdateMotivationBar(_currentMotivation, _maxMotivation);

            Debug.Log($"[HostMotivationBarController] Apresentador recebeu {damageValue} de dano. Motivação atual: {_currentMotivation}/{_maxMotivation}");

            if (_currentMotivation <= 0)
            {
                OnHostDefeated();
            }
        }

        public void UpdateMotivationBar(float currentMotivation, float maxMotivation)
        {
            _currentMotivation = currentMotivation;
            _maxMotivation = maxMotivation;

            if (_motivationSlider != null)
            {
                _motivationSlider.maxValue = _maxMotivation;
                _motivationSlider.value = _currentMotivation;
            }
        }

        private void OnHostDefeated()
        {
            Debug.LogWarning("[HostMotivationBarController] O apresentador foi derrotado!");
        }
    }
}
