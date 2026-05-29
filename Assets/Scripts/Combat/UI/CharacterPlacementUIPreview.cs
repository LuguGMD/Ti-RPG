using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterPlacementUIPreview : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        private int _addedCharacter = 0;

        private void Start()
        {
            UpdateImages();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterCreated += OnCharacterCreated;
            ActionsManager.Instance.OnCombatStart += OnCombatStart;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterCreated -= OnCharacterCreated;
            ActionsManager.Instance.OnCombatStart -= OnCombatStart;
        }

        private void OnCharacterCreated(CharacterController character)
        {
            _addedCharacter++;
            UpdateImages();
        }

        private void OnCombatStart()
        {
            Destroy(gameObject);
        }

        private void UpdateImages()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                if (i < _addedCharacter)
                {
                    _images[i].rectTransform.DOAnchorPosX(130, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        _images[i].enabled = false;
                    });
                }
                else
                {
                    if (i == _addedCharacter)
                    {
                        _images[i].rectTransform.DOAnchorPosX(-80, 1f).SetEase(Ease.OutBack);
                    }

                    _images[i].sprite = GameManager.CurrentParty[i].Icon;
                }
            }
        }
    }
}
