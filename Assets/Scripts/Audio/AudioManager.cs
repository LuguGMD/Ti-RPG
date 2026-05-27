using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Lugu.Singleton;
using RPG.Combat;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG.Audio
{
    public class AudioManager : SingletonMonoPersistent<AudioManager>
    {
        [Header("FMOD Busses Paths")]
        private string masterBusPath = "bus:/";
        private string musicBusPath = "bus:/Music";
        private string sfxBusPath = "bus:/SFX";

        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _soundEffectsBus;

        private EventInstance musicInstance;
        private List<EventInstance> activeSFX = new List<EventInstance>();

        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
            {
                InitializeBusses();
            }
        }


        private void InitializeBusses()
        {
            _masterBus = RuntimeManager.GetBus(masterBusPath);
            _musicBus = RuntimeManager.GetBus(musicBusPath);
            _soundEffectsBus = RuntimeManager.GetBus(sfxBusPath);
        }

        public void PlayMusic(EventReference musicRef)
        {
            if (musicInstance.isValid())
            {
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
            }

            musicInstance = RuntimeManager.CreateInstance(musicRef);
            musicInstance.start();
        }

        public void PlayOneShot(EventReference sfxRef)
        {

            RuntimeManager.PlayOneShot(sfxRef);
        }

        private Bus GetBusType(FMODBusEnum type)
        {
            return type switch
            {
                FMODBusEnum.Master => _masterBus,
                FMODBusEnum.Music => _musicBus,
                FMODBusEnum.SFX => _soundEffectsBus,
                _ => _masterBus
            };
        }

        public float GetVolume(FMODBusEnum type)
        {
            Bus bank = GetBusType(type);
            bank.getVolume(out float volume);
            return volume;
        }

        public void SetVolume(FMODBusEnum type, float volume)
        {
            GetBusType(type).setVolume(volume);
        }

        public bool IsMusicPlaying()
        {
            if (!musicInstance.isValid())
            {
                return false;

            }

            PLAYBACK_STATE state;
            musicInstance.getPlaybackState(out state);
            return state != PLAYBACK_STATE.STOPPED;
        }

        private void OnEnable()
        {
            if (ActionsManager.Instance != null)
            {
                ActionsManager.Instance.OnCharacterCreated += CharacterEntered;
                ActionsManager.Instance.OnCharacterDefeated += CharacterDefeated;
            }
        }

        private void OnDisable()
        {
            if (ActionsManager.Instance != null)
            {
                ActionsManager.Instance.OnCharacterCreated -= CharacterEntered;
                ActionsManager.Instance.OnCharacterDefeated -= CharacterDefeated;
            }
        }

        private void CharacterEntered(CharacterController character)
        {
            string paramName = character.CharacterInfo.FmodParameterName;

            if (string.IsNullOrEmpty(paramName)) return;

            RuntimeManager.StudioSystem.setParameterByName(paramName, 1f);
        }

        private void CharacterDefeated(CharacterController character)
        {
            string paramName = character.CharacterInfo.FmodParameterName;

            if (string.IsNullOrEmpty(paramName)) return;

            RuntimeManager.StudioSystem.setParameterByName(paramName, 0f);
        }
    }
}