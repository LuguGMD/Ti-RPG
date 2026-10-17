using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    public class BossPushAttack : CombatAction
    {
        private PushEffect _pushEffect;

        public override void Init(StageEntityController user)
        {
            _pushEffect = new PushEffect(Grid.DirectionEnum.Down, 1, false);
            Effect effect = new Effect();
            effect.Commands.Add(_pushEffect);
            for (int j = 1; j < Map.Rows; j++)
            {
                for (int i = 0; i < 12; i++)
                {
                    effect.Area.Add(Vector2Int.right * i + Vector2Int.up * j);
                }
            }
            effect.TargetList.Add(TeamEnum.Enemies);
            _effects.Add(effect);
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            yield return new WaitForSeconds(0.5f);
            _effects[0].Execute(_user);
            yield return new WaitForSeconds(0.5f);
        }

        public override List<PreviewTileInfo> Preview()
        {
            List<PreviewTileInfo> previewTilesInfo = new List<PreviewTileInfo>();
            return previewTilesInfo;
        }
    }
}
