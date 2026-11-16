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
        private bool _spritesCached = false;

        public void Init(ActionPreviewTile parentTile, ActionPreviewTile actionTile)
        {
            if (!_spritesCached)
            {
                _damageSprite = _effectSprite.sprite;
                _damageOutlineSprite = _effectOutlineSprite.sprite;
                _spritesCached = true;
            }

            _effectSprite.color = parentTile.Color;
            _effectOutlineSprite.color = parentTile.SecondaryColor;
            _parentTile = parentTile;
            _actionTile = actionTile;

            CheckHits(out bool hitsHeal, out bool hitsDamage, out bool onlyHeals);

            bool showHeal = hitsHeal || (!hitsDamage && onlyHeals);

            bool useHealSprites = showHeal && _healSprite != null && _healOutlineSprite != null;
            _effectSprite.sprite = useHealSprites ? _healSprite : _damageSprite;
            _effectOutlineSprite.sprite = useHealSprites ? _healOutlineSprite : _damageOutlineSprite;

            _effectSprite.enabled = hitsHeal || hitsDamage;
        }

        private void CheckHits(out bool hitsHeal, out bool hitsDamage, out bool onlyHeals)
        {
            hitsHeal = false;
            hitsDamage = false;
            onlyHeals = false;

            if (_parentTile.Info == null) return;

            List<Effect> effects = _parentTile.Info.Effects;
            if (effects.Count == 0) return;

            onlyHeals = true;
            Tile tile = MapManager.Map.GetTile(_actionTile.TilePosition);
            bool occupied = tile != null && tile.IsOccupied;

            foreach (Effect effect in effects)
            {
                bool isHeal = IsHealEffect(effect);
                if (!isHeal) onlyHeals = false;

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
