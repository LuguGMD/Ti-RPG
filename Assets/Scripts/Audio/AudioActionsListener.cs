using UnityEngine;
using Lugu.Singleton;
using FMODUnity;

namespace RPG.Audio
{
    public class AudioActionsListener : SingletonMono<AudioActionsListener>
    {
        [SerializeField] private EventReference _errorSFX;

        private void OnEnable()
        {
            ActionsManager.Instance.OnError += PlayErrorSound;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnError -= PlayErrorSound;
        }

        private void PlayErrorSound()
        {
            AudioManager.Instance.PlayOneShot(_errorSFX);
        }
    }
}
