using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    public class SuperHealHandler : SuperHandler
    {
        private HealingEffect _healEffect;
        private float[] _healAmountTiers = new float[3]
        {
            10,
            20,
            30,
        };

        public override void Init(StageEntityController user)
        {
            _healEffect = new HealingEffect(_healAmountTiers[_upgradeTier], true);
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            yield return new WaitForSeconds(1);

            do
            {
                foreach (Effect effect in root.Effects)
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

            PreviewTileInfo none = new PreviewTileInfo(Vector2Int.zero, Grid.DirectionEnum.None, false);
            firstSteps.Add(none);

            return firstSteps;
        }
    }
}
