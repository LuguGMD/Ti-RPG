using DG.Tweening;
using RPG.Combat.Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CharacterPlacementUIPreview : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        private int _addedCharacter = 0;
        [SerializeField] private Transform _characterPlacement;
        [SerializeField] private Material _previewMaterial;

        private void Start()
        {
            _previewMaterial = new Material(_previewMaterial);
            _previewMaterial.SetFloat("_Fresnel_Intensity", 1);
            _previewMaterial.SetFloat("_Fresnel_Power", 0.2f);
            UpdateImages();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCharacterCreated += OnCharacterCreated;
            ActionsManager.Instance.OnCombatStart += OnCombatStart;
            ActionsManager.Instance.OnTileHovered += OnTileHovered;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCharacterCreated -= OnCharacterCreated;
            ActionsManager.Instance.OnCombatStart -= OnCombatStart;
            ActionsManager.Instance.OnTileHovered -= OnTileHovered;
        }

        private void OnCharacterCreated(CharacterController character)
        {
            _addedCharacter++;
            UpdateImages();
        }

        private void OnCombatStart()
        {
            Destroy(_characterPlacement.gameObject);
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

            ReplacePreviewModel(_addedCharacter);
        }

        private void OnTileHovered(Vector2Int tilePosition)
        {
            _characterPlacement.position = MapManager.Instance.GetWorldPosition(tilePosition);
            Vector3 lookAtPosition = _characterPlacement.position;
            lookAtPosition.y = 0;
            lookAtPosition += lookAtPosition.normalized;
            _characterPlacement.transform.LookAt(_characterPlacement.position + lookAtPosition);
        }

        private void ReplacePreviewModel(int partyMemberIndex)
        {
            if (_addedCharacter >= GameManager.CurrentParty.Length) return;

            foreach (Transform child in _characterPlacement)
            {
                Destroy(child.gameObject);
            }

            CharacterScriptable character = GameManager.CurrentParty[partyMemberIndex];

            GameObject characterModel = Instantiate(character.PreviewModelPrefab.gameObject, _characterPlacement);
            characterModel.transform.localPosition = Vector3.zero;
            characterModel.transform.localRotation = Quaternion.identity;

            Renderer[] renderers = characterModel.GetComponentsInChildren<Renderer>();

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
    }
}
