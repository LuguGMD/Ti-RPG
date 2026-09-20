using UnityEngine;
using Lugu.Singleton;
using FMODUnity;

namespace RPG.Audio
{
    public class AudioActionsListener : SingletonMono<AudioActionsListener>
    {
        [SerializeField] private EventReference _errorSFX;
        [SerializeField] private EventReference _healSFX;
        private bool _didPlayHealSound = false;

        private void OnEnable()
        {
            ActionsManager.Instance.OnError += PlayErrorSound;
            ActionsManager.Instance.OnCharacterHealed += PlayHealSound;
            ActionsManager.Instance.OnActionEnd += OnActionEnd;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnError -= PlayErrorSound;
            ActionsManager.Instance.OnCharacterHealed -= PlayHealSound;
            ActionsManager.Instance.OnActionEnd -= OnActionEnd;
        }

        private void OnActionEnd()
        {
            _didPlayHealSound = false;
        }

        private void PlayErrorSound()
        {
            AudioManager.Instance.PlayOneShot(_errorSFX);
        }

        private void PlayHealSound(RPG.Combat.CharacterController character)
        {
            if (_didPlayHealSound) return;

            AudioManager.Instance.PlayOneShot(_healSFX);
            _didPlayHealSound = true;
        }
    }
}
