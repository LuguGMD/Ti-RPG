using RPG.Combat.Actions;
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

        public void Init(ActionPreviewTile parentTile, ActionPreviewTile actionTile)
        {
            _effectSprite.color = parentTile.Color;
            _effectOutlineSprite.color = parentTile.SecondaryColor;
            _parentTile = parentTile;
            _actionTile = actionTile;

            if(IsHittingTarget())
            {
                _effectSprite.enabled = true;
            }
            else
            {
                _effectSprite.enabled = false;
            }
        }

        private bool IsHittingTarget()
        {
            if (_parentTile.Info == null) return false;

            Effect effect = _parentTile.Info.Effects[0];
            List<TeamEnum> targets = effect.TargetList;
            foreach (var target in targets)
            {
                Tile tile = MapManager.Map.GetTile(_actionTile.TilePosition);
                if (tile != null && tile.IsOccupied)
                {

                    if (tile.TileObject.Entity.Info.Team == target)
                    {
                        //TO DO adicionar verificao de targetself
                        //if(effect.CanTargetSelf && )

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
