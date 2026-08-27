using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    public class SuperPushHandler : SuperHandler
    {
        private PushEffect _pushEffect;
        private int[] _pushAmountTiers = new int[3]
        {
            1,
            2,
            3,
        };

        public override void Init(StageEntityController user)
        {
            _user = user;
            _pushEffect = new PushEffect(Grid.DirectionEnum.Up, _pushAmountTiers[_upgradeTier], false);
            _effects.Add(new Effect());
            _effects[0].Commands.Add(_pushEffect);
            _effects[0].TargetList.Add(TeamEnum.Enemies);
            _effects[0].Area.Add(Vector2Int.up);
            for (int i = 1; i < Map.Columns; i++)
            {
                _effects[0].Area.Add(Vector2Int.up + Vector2Int.right * i);
            }
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            yield return new WaitForSeconds(1);

            do
            {
                foreach (Effect effect in _effects)
                {
                    effect.Execute(_user);
                }
                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);
        }


        public override List<PreviewTileInfo> Preview()
        {
            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            PreviewTileInfo up = new PreviewTileInfo(Vector2Int.up, Grid.DirectionEnum.Up, false, false, false, true);
            firstSteps.Add(up);

            return firstSteps;
        }
    }
}
