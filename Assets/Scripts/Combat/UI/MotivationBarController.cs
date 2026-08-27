using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

namespace RPG.Combat.UI
{
    public class MotivationBarController : MonoBehaviour
    {
        [SerializeField] private CharacterMotivationSlider _motivationBarPrefab;
        [SerializeField] private Transform _motivationBarContainer;
        [SerializeField] private Image _apresentadorIcon;
        [SerializeField] private Sprite _apresentadorIconSprite;
        [SerializeField] private Sprite _apresentadorUsedIconSprite;

        [SerializeField] private Slider _apresentadorSlider;

        private Dictionary<CharacterController, CharacterMotivationSlider> _characterSliders = new Dictionary<CharacterController, CharacterMotivationSlider>();

        private void Start()
        {
            _apresentadorSlider.value = 0f;
            _apresentadorSlider.DOValue(1f, 1.5f);
        }
        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterCreated += AddCharacter;
            ActionsManager.Instance.OnCharacterDefeated += RemoveCharacter;
            ActionsManager.Instance.OnCharacterDamageTaken += UpdateCharacterHealth;
            ActionsManager.Instance.OnCharacterHealed += UpdateCharacterHealth;
            ActionsManager.Instance.OnApresentadorDamageTaken += UpdateApresentadorDamage;
            ActionsManager.Instance.OnApresentadorHealed += UpdateApresentadorDamage;
            ActionsManager.Instance.OnActionStart += UpdateInfo;
            ActionsManager.Instance.OnApresentadorActionCompleted += UpdateInfo;
            ActionsManager.Instance.OnPlayerTurnStarted += UpdateInfo;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterCreated -= AddCharacter;
            ActionsManager.Instance.OnCharacterDefeated -= RemoveCharacter;
            ActionsManager.Instance.OnCharacterDamageTaken -= UpdateCharacterHealth;
            ActionsManager.Instance.OnCharacterHealed -= UpdateCharacterHealth;
            ActionsManager.Instance.OnApresentadorDamageTaken -= UpdateApresentadorDamage;
            ActionsManager.Instance.OnApresentadorHealed -= UpdateApresentadorDamage;
            ActionsManager.Instance.OnActionStart -= UpdateInfo;
            ActionsManager.Instance.OnApresentadorActionCompleted -= UpdateInfo;
            ActionsManager.Instance.OnPlayerTurnStarted -= UpdateInfo;
        }

        private void AddCharacter(CharacterController character)
        {
            CharacterMotivationSlider motivationSlider = Instantiate<CharacterMotivationSlider>(_motivationBarPrefab, _motivationBarContainer);
            motivationSlider.SetInfo(character);

            _characterSliders.Add(character, motivationSlider);

            UpdateCharacterHealth(character);
        }

        private void RemoveCharacter(CharacterController character)
        {
            if(_characterSliders.ContainsKey(character))
            {
                Destroy(_characterSliders[character].gameObject);
                _characterSliders.Remove(character);
            }
        }

        private void UpdateInfo()
        {
            foreach(CharacterController character in CombatManager.RemainingCharacters)
            {
                if(_characterSliders.TryGetValue(character, out CharacterMotivationSlider slider))
                {
                    slider.SetInfo(character);
                }
            }

            _apresentadorIcon.sprite = CombatManager.Apresentador.HasActed ? _apresentadorUsedIconSprite : _apresentadorIconSprite;
        }

        private void UpdateCharacterHealth(CharacterController character)
        {
            if (!_characterSliders.ContainsKey(character)) return;

            CharacterMotivationSlider motivationSlider = _characterSliders[character];
            motivationSlider.Slider.DOValue((CombatConstants.MAX_MOTIVATION_APRESENTADOR - character.CurrentMotivation) / CombatConstants.MAX_MOTIVATION_APRESENTADOR, 1f);
        }

        private void UpdateApresentadorDamage()
        {
            _apresentadorSlider.DOValue(CombatManager.Apresentador.CurrentMotivation / CombatConstants.MAX_MOTIVATION_APRESENTADOR, 0.2f);
        }
    }
}
