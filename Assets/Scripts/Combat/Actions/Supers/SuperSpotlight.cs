using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    public class SuperSpotlight : SuperHandler
    {
        /*private int[] _chargeAmountTiers = new int[3]
        {
            10,
            6,
            3,
        };*/

        private int[] _chargeAmountTiers = new int[3]
        {
            1,
            1,
            1,
        };

        public override void Init(StageEntityController user)
        {
            _user = user;
            _chargeAmount = _chargeAmountTiers[_upgradeTier];
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            Vector2Int previousSpotlightPosition = MapManager.SpotlightPosition;

            ActionsManager.Instance.OnSpotlightSuperStarted?.Invoke();
            if(SpotlightHandler.Instance != null)
                yield return new WaitUntil(() => SpotlightHandler.Instance.IsSuperActive == false);
            yield return new WaitForSeconds(1f);
        }


        public override List<PreviewTileInfo> Preview()
        {
            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            PreviewTileInfo up = new PreviewTileInfo(Vector2Int.up * 2, Grid.DirectionEnum.Up, false, false, false, true);
            firstSteps.Add(up);

            PreviewTileInfo child = up.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, false, false, false, true);
            firstSteps.Add(child);

            for (int i = 2; i < Map.Columns; i++)
            {
                child = child.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, false, false, false, true);
                firstSteps.Add(child);
            }

            return firstSteps;
        }
    }
}
