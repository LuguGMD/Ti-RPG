using RPG.Combat.Actions;
using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Preview
{
    public class DamagePreviewHandler : MonoBehaviour
    {
        private ActionPreviewTile _parentTile;
        private ActionPreviewTile _actionTile;
        [SerializeField] private SpriteRenderer _effectSprite;
        [SerializeField] private SpriteRenderer _effectOutlineSprite;
        [Header("Heal Sprites")]
        [SerializeField] private Sprite _healSprite;
        [SerializeField] private Sprite _healOutlineSprite;

        private Sprite _damageSprite;
        private Sprite _damageOutlineSprite;
        private Vector3 _baseLocalPosition;
        private Vector3 _baseLocalScale;
        private bool _initialized = false;

        private GameObject _healObject;
        private SpriteRenderer _healEffectSprite;
        private SpriteRenderer _healEffectOutlineSprite;

        public void Init(ActionPreviewTile parentTile, ActionPreviewTile actionTile)
        {
            if (!_initialized)
            {
                _damageSprite = _effectSprite.sprite;
                _damageOutlineSprite = _effectOutlineSprite.sprite;
                _baseLocalPosition = transform.localPosition;
                _baseLocalScale = transform.localScale;
                _initialized = true;
            }

            _effectSprite.color = parentTile.Color;
            _effectOutlineSprite.color = parentTile.SecondaryColor;
            _parentTile = parentTile;
            _actionTile = actionTile;

            CheckHits(out bool hasHeal, out bool hasDamage, out bool hitsHeal, out bool hitsDamage);

            bool canShowHeal = _healSprite != null && _healOutlineSprite != null;

            if (hasHeal && hasDamage && canShowHeal)
            {
                ShowBoth(hitsHeal, hitsDamage);
            }
            else if (hasHeal && canShowHeal)
            {
                ShowSingle(_healSprite, _healOutlineSprite, hitsHeal);
            }
            else
            {
                ShowSingle(_damageSprite, _damageOutlineSprite, hitsDamage);
            }
        }

        private void ShowSingle(Sprite fill, Sprite outline, bool isHitting)
        {
            ResetLayout();

            _effectSprite.sprite = fill;
            _effectOutlineSprite.sprite = outline;
            _effectSprite.enabled = isHitting;
        }

        private void ShowBoth(bool hitsHeal, bool hitsDamage)
        {
            CreateHealObject();

            _effectSprite.sprite = _damageSprite;
            _effectOutlineSprite.sprite = _damageOutlineSprite;
            _effectSprite.enabled = hitsDamage;

            _healEffectSprite.sprite = _healSprite;
            _healEffectOutlineSprite.sprite = _healOutlineSprite;
            _healEffectSprite.color = _effectSprite.color;
            _healEffectOutlineSprite.color = _effectOutlineSprite.color;
            _healEffectSprite.enabled = hitsHeal;

            float width = _damageSprite.bounds.size.x;
            transform.localScale = _baseLocalScale * 0.5f;
            transform.localPosition = _baseLocalPosition - Vector3.right * (width * _baseLocalScale.x * 0.25f);

            _healObject.transform.localPosition = new Vector3(width, 0f, 0f);
            _healObject.SetActive(true);
        }

        private void ResetLayout()
        {
            transform.localScale = _baseLocalScale;
            transform.localPosition = _baseLocalPosition;

            if (_healObject != null)
                _healObject.SetActive(false);
        }

        private void CreateHealObject()
        {
            if (_healObject != null) return;

            _healObject = Instantiate(_effectSprite.gameObject, transform);
            _healObject.name = "HealPreview";
            Destroy(_healObject.GetComponent<DamagePreviewHandler>());

            _healObject.transform.localRotation = Quaternion.identity;
            _healObject.transform.localScale = Vector3.one;

            SpriteRenderer[] renderers = _healObject.GetComponentsInChildren<SpriteRenderer>(true);
            _healEffectSprite = renderers[0];
            _healEffectOutlineSprite = renderers[1];
        }

        private void CheckHits(out bool hasHeal, out bool hasDamage, out bool hitsHeal, out bool hitsDamage)
        {
            hasHeal = false;
            hasDamage = false;
            hitsHeal = false;
            hitsDamage = false;

            if (_parentTile.Info == null) return;

            Tile tile = MapManager.Map.GetTile(_actionTile.TilePosition);
            bool occupied = tile != null && tile.IsOccupied;

            foreach (Effect effect in _parentTile.Info.Effects)
            {
                bool isHeal = IsHealEffect(effect);
                if (isHeal) hasHeal = true;
                else hasDamage = true;

                if (!occupied) continue;

                if (effect.TargetList.Contains(tile.TileObject.Entity.Info.Team))
                {
                    //TO DO adicionar verificao de targetself
                    if (isHeal) hitsHeal = true;
                    else hitsDamage = true;
                }
            }
        }

        private static bool IsHealEffect(Effect effect)
        {
            foreach (EffectCommand command in effect.Commands)
            {
                if (command is HealingEffect) return true;
            }
            return false;
        }
    }
}
