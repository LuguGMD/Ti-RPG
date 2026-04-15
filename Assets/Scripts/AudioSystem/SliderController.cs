using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    [RequireComponent(typeof(Slider))]
    public class SliderController : MonoBehaviour
    {
        [Tooltip("Configuração de Slider - Altera os bancos do FMOD (Master, Music, SFX)")]
        [SerializeField] private FMODBanks bankType;

        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            if (AudioManager.Instance != null)
            {
                slider.value = AudioManager.Instance.GetVolume(bankType);
            }

            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetVolume(bankType, value);
            }
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
