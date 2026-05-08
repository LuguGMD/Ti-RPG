using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPG.Combat.UI
{
    public class MotivationBarController : MonoBehaviour
    {
        [SerializeField] private CharacterMotivationSlider _motivationBarPrefab;
        [SerializeField] private Transform _motivationBarContainer;

        [SerializeField] private Slider _apresentadorSlider;

        private Dictionary<CharacterController, CharacterMotivationSlider> _characterSliders = new Dictionary<CharacterController, CharacterMotivationSlider>();

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterCreated += AddCharacter;
            ActionsManager.Instance.OnCharacterDefeated += RemoveCharacter;
            ActionsManager.Instance.OnCharacterDamageTaken += UpdateCharacterDamage;
            ActionsManager.Instance.OnApresentadorDamageTaken += UpdateApresentadorDamage;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterCreated -= AddCharacter;
            ActionsManager.Instance.OnCharacterDefeated -= RemoveCharacter;
            ActionsManager.Instance.OnCharacterDamageTaken -= UpdateCharacterDamage;
            ActionsManager.Instance.OnApresentadorDamageTaken -= UpdateApresentadorDamage;
        }

        private void AddCharacter(CharacterController character)
        {
            CharacterMotivationSlider motivationSlider = Instantiate<CharacterMotivationSlider>(_motivationBarPrefab, _motivationBarContainer);
            motivationSlider.SetInfo(character.CharacterInfo);

            _characterSliders.Add(character, motivationSlider);

            UpdateCharacterDamage(character);
        }

        private void RemoveCharacter(CharacterController character)
        {
            if(_characterSliders.ContainsKey(character))
            {
                Destroy(_characterSliders[character].gameObject);
                _characterSliders.Remove(character);
            }
        }

        private void UpdateCharacterDamage(CharacterController character)
        {
            if (!_characterSliders.ContainsKey(character)) return;

            CharacterMotivationSlider motivationSlider = _characterSliders[character];
            motivationSlider.Slider.value = (CombatConstants.MAX_MOTIVATION_APRESENTADOR - character.CurrentMotivation) / CombatConstants.MAX_MOTIVATION_APRESENTADOR;
        }

        private void UpdateApresentadorDamage()
        {
            _apresentadorSlider.value = CombatManager.Apresentador.CurrentMotivation / CombatConstants.MAX_MOTIVATION_APRESENTADOR;
        }
    }
}
