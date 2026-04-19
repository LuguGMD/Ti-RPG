using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPG
{
    public class CharacterMotivationBarController : MonoBehaviour
    {
        [SerializeField] private Slider _damageBarPrefab;
        [SerializeField] private Transform _damageBarContainer;
        [SerializeField] private int _maxCharacters = 4;

        private List<CharacterDamageBar> _characterDamageBars = new List<CharacterDamageBar>();

        private class CharacterDamageBar
        {
            public int CharacterIndex;
            public Slider DamageSlider;
            public float AccumulatedDamage;
            public bool IsAlive;
        }

        #region Properties

        public int ActiveCharacterCount { get { return _characterDamageBars.Count; } }

        #endregion

        public void Initialize()
        {
            if (_damageBarPrefab == null || _damageBarContainer == null)
            {
                Debug.LogError("[CharacterMotivationBarController] Initialize: DamageBarPrefab ou DamageBarContainer não foi atribuído!");
                return;
            }

        }

        public void AddCharacter(int characterIndex, string characterName = "")
        {
            if (_characterDamageBars.Count >= _maxCharacters || _characterDamageBars.Find(c => c.CharacterIndex == characterIndex) != null)
            {
                Debug.LogWarning($"[CharacterMotivationBarController] AddCharacter: Limite de {_maxCharacters} personagens atingido!");
                return;
            }

            Slider newDamageBar = Instantiate(_damageBarPrefab, _damageBarContainer);
            newDamageBar.name = $"CharacterDamageBar_{characterIndex}_{characterName}";
            newDamageBar.maxValue = 100f;
            newDamageBar.value = 0;
            newDamageBar.interactable = false;

            CharacterDamageBar characterDamageBar = new CharacterDamageBar
            {
                CharacterIndex = characterIndex,
                DamageSlider = newDamageBar,
                AccumulatedDamage = 0,
                IsAlive = true
            };

            _characterDamageBars.Add(characterDamageBar);

            Debug.Log($"[CharacterMotivationBarController] AddCharacter: Personagem {characterIndex} ({characterName}) adicionado com sucesso.");
        }

        public void RemoveCharacter(int characterIndex)
        {
            CharacterDamageBar characterDamageBar = _characterDamageBars.Find(c => c.CharacterIndex == characterIndex);

            if (characterDamageBar == null)
            {
                Debug.LogWarning($"[CharacterMotivationBarController] RemoveCharacter: Personagem {characterIndex} não foi encontrado!");
                return;
            }

            Destroy(characterDamageBar.DamageSlider.gameObject);
            _characterDamageBars.Remove(characterDamageBar);

            Debug.Log($"[CharacterMotivationBarController] RemoveCharacter: Personagem {characterIndex} removido com sucesso.");
        }

        public void UpdateCharacterDamage(int characterIndex, float damageValue, float maxHostMotivation)
        {
            CharacterDamageBar characterDamageBar = _characterDamageBars.Find(c => c.CharacterIndex == characterIndex);

            if (characterDamageBar == null)
            {
                Debug.LogWarning($"[CharacterMotivationBarController] UpdateCharacterDamage: Personagem {characterIndex} não foi encontrado!");
                return;
            }

            characterDamageBar.AccumulatedDamage += damageValue;

            if (characterDamageBar.DamageSlider != null)
            {
                characterDamageBar.DamageSlider.maxValue = maxHostMotivation;
                characterDamageBar.DamageSlider.value = characterDamageBar.AccumulatedDamage;
            }

            Debug.Log($"[CharacterMotivationBarController] UpdateCharacterDamage: Personagem {characterIndex} recebeu {damageValue} de dano. Dano acumulado: {characterDamageBar.AccumulatedDamage}/{maxHostMotivation}");

            if (characterDamageBar.AccumulatedDamage > maxHostMotivation && characterDamageBar.IsAlive)
            {
                OnCharacterDefeated(characterIndex);
            }
        }

        public bool IsCharacterAlive(int characterIndex)
        {
            CharacterDamageBar characterDamageBar = _characterDamageBars.Find(c => c.CharacterIndex == characterIndex);

            if (characterDamageBar == null)
            {
                Debug.LogWarning($"[CharacterMotivationBarController] IsCharacterAlive: Personagem {characterIndex} não foi encontrado!");
                return false;
            }

            return characterDamageBar.IsAlive;
        }

        private void OnCharacterDefeated(int characterIndex)
        {
            CharacterDamageBar characterDamageBar = _characterDamageBars.Find(c => c.CharacterIndex == characterIndex);

            if (characterDamageBar != null)
            {
                characterDamageBar.IsAlive = false;

                Debug.LogWarning($"[CharacterMotivationBarController] OnCharacterDefeated: Personagem {characterIndex} foi derrotado! Dano acumulado ({characterDamageBar.AccumulatedDamage}) ultrapassou o limite!");
            }
        }

        public void ResetCharacterDamage(int characterIndex)
        {
            CharacterDamageBar characterDamageBar = _characterDamageBars.Find(c => c.CharacterIndex == characterIndex);

            if (characterDamageBar == null)
            {
                Debug.LogWarning($"[CharacterMotivationBarController] ResetCharacterDamage: Personagem {characterIndex} não foi encontrado!");
                return;
            }

            characterDamageBar.AccumulatedDamage = 0;
            characterDamageBar.IsAlive = true;

            if (characterDamageBar.DamageSlider != null)
            {
                characterDamageBar.DamageSlider.value = 0;
            }

            Debug.Log($"[CharacterMotivationBarController] ResetCharacterDamage: Dano do personagem {characterIndex} foi resetado.");
        }

        public void ClearAllCharacters()
        {
            foreach (CharacterDamageBar characterDamageBar in _characterDamageBars)
            {
                if (characterDamageBar.DamageSlider != null)
                {
                    Destroy(characterDamageBar.DamageSlider.gameObject);
                }
            }

            _characterDamageBars.Clear();

            Debug.Log("[CharacterMotivationBarController] ClearAllCharacters: Todos os personagens foram removidos.");
        }
    }
}
