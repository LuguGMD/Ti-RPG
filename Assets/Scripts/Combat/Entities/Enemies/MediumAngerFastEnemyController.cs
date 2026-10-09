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
        public const int ATTACK_DISTANCE = 3;

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

            _attack.ChangeDirection(Vector2Int.down);

            List<ActionTargetInfo> targets = GetActionTargets();

            if (targets.Count > 0)
            {
                int index = Random.Range(0, targets.Count%4);
                _attack.ChangeDirection(targets[index].TargetDirection);
            }

            List<PreviewTileInfo> tiles = _attack.Preview();
            _preparedAction = PreviewTileInfo.GetLeaf(tiles[0]);
            if (_preparedAction.Parent != null) _preparedAction = _preparedAction.Parent;

            SelectAction(0);
        }

        protected override List<ActionTargetInfo> GetActionTargets()
        {
            List<ActionTargetInfo> targets = new List<ActionTargetInfo>();

            int distance = ATTACK_DISTANCE;
            for (int i = distance; i > 0; i--)
            {
                StageEntityController entity;

                entity = MapManager.Map.GetTile(Position + (Vector2Int.left * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) targets.Add(new ActionTargetInfo(this, entity as CharacterController, Vector2Int.left, _attack.Damage));
                entity = MapManager.Map.GetTile(Position + (Vector2Int.right * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) targets.Add(new ActionTargetInfo(this, entity as CharacterController, Vector2Int.right, _attack.Damage));
                entity = MapManager.Map.GetTile(Position + (Vector2Int.up * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) targets.Add(new ActionTargetInfo(this, entity as CharacterController, Vector2Int.up, _attack.Damage));
                entity = MapManager.Map.GetTile(Position + (Vector2Int.down * i))?.TileObject?.Entity;
                if (entity != null && entity.Info.Team == TeamEnum.Circus) targets.Add(new ActionTargetInfo(this, entity as CharacterController, Vector2Int.down, _attack.Damage));
            }

            return targets;
        }
    }
}
