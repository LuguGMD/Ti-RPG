using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class MediumAngerFastEnemyController : EnemyController
    {
        [SerializeField] private MediumAngerAttack _attack;

        protected override void InitCombatActions()
        {
            if (_actions.Count == 0)
            {
                _actions.Add(_attack);
                base.InitCombatActions();
            }
        }

        public override void PrepareAction()
        {
            if (_actions.Count == 0)
            {
                InitCombatActions();
            }

            SelectAction(0);

            //TO DO verificar personagem proximo
            _attack.ChangeDirection(Vector2Int.down);
            List<Vector2Int> _closeTargetDirections = new List<Vector2Int>();
            int distance = 3;

            for(int i = distance; i > 0; i--)
            {
                StageEntityController entity;

                entity = MapManager.Map.GetTile(Position + (Vector2Int.left * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) _closeTargetDirections.Add((Vector2Int.left));
                entity = MapManager.Map.GetTile(Position + (Vector2Int.right * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) _closeTargetDirections.Add((Vector2Int.right));
                entity = MapManager.Map.GetTile(Position + (Vector2Int.up * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) _closeTargetDirections.Add((Vector2Int.up));
                entity = MapManager.Map.GetTile(Position + (Vector2Int.down * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) _closeTargetDirections.Add((Vector2Int.down));

                if (_closeTargetDirections.Count > 0) break;
            }


            if (_closeTargetDirections.Count > 0)
            {
                int index = Random.Range(0, _closeTargetDirections.Count);
                _attack.ChangeDirection(_closeTargetDirections[index]);
            }

            List<PreviewTileInfo> tiles = _attack.Preview();
            _preparedAction = PreviewTileInfo.GetLeaf(tiles[0]);
            if (_preparedAction.Parent != null) _preparedAction = _preparedAction.Parent;

            SelectAction(0);
        }
    }
}
