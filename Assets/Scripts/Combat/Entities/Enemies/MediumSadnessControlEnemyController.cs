using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class MediumSadnessControlEnemyController : EnemyController
    {
        [SerializeField] private MediumSadnessRegularAttack _regularAttack;
        [SerializeField] private MediumSadnessScreamAttack _screamAttack;
        private bool _wasDamaged = false;

        public override void ResetAction()
        {
            base.ResetAction();
            _wasDamaged = false;
        }

        protected override void InitCombatActions()
        {
            if (_actions.Count == 0)
            {
                _actions.Add(_regularAttack);
                _actions.Add(_screamAttack);
                base.InitCombatActions();
            }
        }

        public override void PrepareAction()
        {
            if (_actions.Count == 0)
            {
                InitCombatActions();
            }

            if (!_wasDamaged)
            {

                SelectAction(0);

                _regularAttack.ChangeDirection(Vector2Int.down);
                List<Vector2Int> _closeTargetDirections = new List<Vector2Int>();
                int distance = 1;

                for (int i = distance; i > 0; i--)
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
                    _regularAttack.ChangeDirection(_closeTargetDirections[index]);
                }

                List<PreviewTileInfo> tiles = _regularAttack.Preview();
                _preparedAction = tiles[0];

                SelectAction(0);
            }
            else
            {
                List<PreviewTileInfo> tiles = _screamAttack.Preview();
                _preparedAction = tiles[0];
                SelectAction(1);
            }
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            _wasDamaged = true;
        }
    }
}
