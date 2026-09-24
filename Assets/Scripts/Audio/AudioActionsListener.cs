using UnityEngine;
using Lugu.Singleton;
using FMODUnity;
using UnityEngine.TextCore.Text;

namespace RPG.Audio
{
    public class AudioActionsListener : SingletonMono<AudioActionsListener>
    {
        [SerializeField] private EventReference _errorSFX;
        [SerializeField] private EventReference _healSFX;
        [SerializeField] private EventReference _spotlightBuffSFX;
        [SerializeField] private EventReference _spotlightEnemyBuffSFX;
        private bool _didPlayHealSound = false;

        private void OnEnable()
        {
            ActionsManager.Instance.OnError += PlayErrorSound;
            ActionsManager.Instance.OnCharacterHealed += PlayHealSound;
            ActionsManager.Instance.OnActionEnd += OnActionEnd;
            ActionsManager.Instance.OnCharacterActionUsed += OnCharacterActionUsed;
            ActionsManager.Instance.OnEnemyActionUsed += OnEnemyActionUsed;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnError -= PlayErrorSound;
            ActionsManager.Instance.OnCharacterHealed -= PlayHealSound;
            ActionsManager.Instance.OnActionEnd -= OnActionEnd;
            ActionsManager.Instance.OnCharacterActionUsed -= OnCharacterActionUsed;
            ActionsManager.Instance.OnEnemyActionUsed -= OnEnemyActionUsed;
        }

        private void OnActionEnd()
        {
            _didPlayHealSound = false;
        }

        private void OnCharacterActionUsed(RPG.Combat.CharacterController character)
        {
            if (character.TileObject.IsOnSpotlight == true)
            {
                AudioManager.Instance.PlayOneShot(_spotlightBuffSFX);
            }
        }

        private void OnEnemyActionUsed(RPG.Combat.EnemyController enemy)
        {
            if (enemy.TileObject.IsOnSpotlight == true)
            {
                AudioManager.Instance.PlayOneShot(_spotlightEnemyBuffSFX);
            }
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
