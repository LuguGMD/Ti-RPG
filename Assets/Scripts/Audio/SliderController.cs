using RPG.Save;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Audio
{
    [RequireComponent(typeof(Slider))]
    public class SliderController : MonoBehaviour
    {
        [Tooltip("Configuração de Slider - Altera os bancos do FMOD (Master, Music, SFX)")]
        [SerializeField] private FMODBusEnum busType;

        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetVolume(busType, value);
            }
        }

        private void Load()
        {
            float value = 1;

            switch (busType)
            {
                case FMODBusEnum.Master:
                    value = SaveManager.SaveData.AudioManagerData.MasterVolume;
                    break;
                case FMODBusEnum.Music:
                    value = SaveManager.SaveData.AudioManagerData.MusicVolume;
                    break;
                case FMODBusEnum.SFX:
                    value = SaveManager.SaveData.AudioManagerData.SoundEffectVolume;
                    break;
            }

            slider.value = value;
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnEnable()
        {
            //if (!didStart) return;
            Load();
        }

        private void OnDisable()
        {
            if (!didStart) return;

            SaveManager.Instance.SaveAll();
        }
    }
}
