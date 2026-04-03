using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace RPG
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        [Header("Event References")]

        [SerializeField] private List<EventReference> musicEvents;
        [SerializeField] private List<EventReference> sfxEvents;

        private EventInstance musicInstance;
        private List<EventInstance> activeSFX = new List<EventInstance>();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayMusic(int index)
        {
            if (musicInstance.isValid())
            {
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
            }

            if (index < musicEvents.Count)
            {
                musicInstance = RuntimeManager.CreateInstance(musicEvents[index]);
                musicInstance.start();
            }
        }

        public void PlayOneShot(int index)
        {
            if (index < sfxEvents.Count)
            {
                RuntimeManager.PlayOneShot(sfxEvents[index]);
            }
        }
    }
}
