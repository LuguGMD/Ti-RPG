using DG.Tweening;
using RPG.Combat.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterPlacementUIPreview : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        private List<CharacterScriptable> _charactersToAdd = new List<CharacterScriptable>();
        [SerializeField] private Transform _characterPlacement;
        [SerializeField] private Material _previewMaterial;

        private void Start()
        {
            _previewMaterial = new Material(_previewMaterial);
            _previewMaterial.SetFloat("_Fresnel_Intensity", 1);
            _previewMaterial.SetFloat("_Fresnel_Power", 0.2f);

            for (int i = 0; i < GameManager.CurrentParty.Length; i++)
            {
                _charactersToAdd.Add(GameManager.CurrentParty[i]);
            }

            UpdateImages();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterCreated += OnCharacterCreated;
            ActionsManager.Instance.OnCombatStart += DisablePlacement;
            ActionsManager.Instance.OnTileHovered += OnTileHovered;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterCreated -= OnCharacterCreated;
            ActionsManager.Instance.OnCombatStart -= DisablePlacement;
            ActionsManager.Instance.OnTileHovered -= OnTileHovered;
        }

        private void OnCharacterCreated(CharacterController character)
        {
            _charactersToAdd.Remove(character.CharacterInfo);
            UpdateImages();
        }

        public void DisablePlacement()
        {
            _characterPlacement.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _charactersToAdd.Clear();
        }

        public void EnablePlacement()
        {
            _characterPlacement.gameObject.SetActive(true);
            UpdateImages();
        }

        private void UpdateImages()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                if (i < CombatConstants.MAX_CHARACTERS_COUNT - _charactersToAdd.Count)
                {
                    _images[i].rectTransform.DOAnchorPosX(130, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        _images[i].enabled = false;
                    });
                }
                else
                {
                    if (i == CombatConstants.MAX_CHARACTERS_COUNT - _charactersToAdd.Count)
                    {
                        _images[i].rectTransform.DOAnchorPosX(-80, 1f).SetEase(Ease.OutBack);
                    }

                    _images[i].sprite = _charactersToAdd[0].Icon;
                }
            }

            if (_charactersToAdd.Count > 0)
            {
                ReplacePreviewModel(_charactersToAdd[0]);
            }
        }

        private void OnTileHovered(Vector2Int tilePosition)
        {
            _characterPlacement.position = MapManager.Instance.GetWorldPosition(tilePosition);
            Vector3 lookAtPosition = _characterPlacement.position;
            lookAtPosition.y = 0;
            lookAtPosition += lookAtPosition.normalized;
            _characterPlacement.transform.LookAt(_characterPlacement.position + lookAtPosition);
        }

        private void ReplacePreviewModel(CharacterScriptable character)
        {
            foreach (Transform child in _characterPlacement)
            {
                Destroy(child.gameObject);
            }

            GameObject characterModel = Instantiate(character.PreviewModelPrefab.gameObject, _characterPlacement);
            characterModel.transform.localPosition = Vector3.zero;
            characterModel.transform.localRotation = Quaternion.identity;

            Renderer[] renderers = characterModel.GetComponentsInChildren<Renderer>();

            _previewMaterial.SetColor("_Color", character.EntityPlacementPreviewColor);
            foreach (Renderer renderer in renderers)
            {
                List<Material> materials = new List<Material>();
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    materials.Add(_previewMaterial);
                }
                renderer.SetMaterials(materials);
            }
        }

        public void AddCharacter(CharacterScriptable character)
        {
            if (!_charactersToAdd.Contains(character))
                _charactersToAdd.Add(character);
        }
    }
}
