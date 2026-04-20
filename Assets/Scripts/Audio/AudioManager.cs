using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Lugu.Singleton;

namespace RPG.Audio
{
    public class AudioManager : SingletonMonoPersistent<AudioManager>
    {
        [Header("Sound Setup (Inspector)")]
        [SerializeField] private List<SoundEntry> musicList;
        [SerializeField] private List<SoundEntry> sfxList;

        private Dictionary<string, EventReference> musicDictionary;
        private Dictionary<string, EventReference> sfxDictionary;

        [Header("FMOD Busses Paths")]
        [SerializeField] private string masterBusPath = "bus:/";
        [SerializeField] private string musicBusPath = "bus:/Music";
        [SerializeField] private string sfxBusPath = "bus:/SFX";

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
                InitializeDictionaries();
                InitializeBusses();
            }
        }

        private void InitializeDictionaries()
        {
            musicDictionary = new Dictionary<string, EventReference>();
            sfxDictionary = new Dictionary<string, EventReference>();

            foreach (var music in musicList)
            {
                if (!musicDictionary.ContainsKey(music.soundID))
                {
                    musicDictionary.Add(music.soundID, music.eventReference);
                }
            }

            foreach (var sfx in sfxList)
            {
                if (!sfxDictionary.ContainsKey(sfx.soundID))
                {
                    sfxDictionary.Add(sfx.soundID, sfx.eventReference);
                }
            }
        }

        private void InitializeBusses()
        {
            _masterBus = RuntimeManager.GetBus(masterBusPath);
            _musicBus = RuntimeManager.GetBus(musicBusPath);
            _soundEffectsBus = RuntimeManager.GetBus(sfxBusPath);
        }

        public void PlayMusic(string soundID)
        {
            if (musicDictionary.TryGetValue(soundID, out EventReference musicRef))
            {
                if (musicInstance.isValid())
                {
                    musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    musicInstance.release();
                }

                musicInstance = RuntimeManager.CreateInstance(musicRef);
                musicInstance.start();
            }
            else
            {
                Debug.LogWarning($"AudioManager: Música com ID '{soundID}' não encontrada no dicionário.");
            }
        }

        public void PlayOneShot(string soundID)
        {
            if (sfxDictionary.TryGetValue(soundID, out EventReference sfxRef))
            {
                RuntimeManager.PlayOneShot(sfxRef);
            }
            else
            {
                Debug.LogWarning($"AudioManager: SFX com ID '{soundID}' não encontrado no dicionário.");
            }
        }

        public float GetVolume(FMODBusEnum type)
        {
            Bus bank = _masterBus;

            switch (type)
            {
                case FMODBusEnum.Music:
                    bank = _musicBus;
                    break;
                case FMODBusEnum.SFX:
                    bank = _soundEffectsBus;
                    break;
            }

            bank.getVolume(out float volume);

            return volume;
        }

        public void SetVolume(FMODBusEnum type, float volume)
        {
            Bus bank = _masterBus;

            switch (type)
            {
                case FMODBusEnum.Master:
                    bank = _masterBus;
                    break;
                case FMODBusEnum.Music:
                    bank = _musicBus;
                    break;
                case FMODBusEnum.SFX:
                    bank = _soundEffectsBus;
                    break;
            }
            bank.setVolume(volume);
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
    }
}