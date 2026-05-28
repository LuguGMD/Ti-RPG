using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class EnemyNothingAction : CombatAction
    {
        public override void Init(StageEntityController user)
        {
            _user = user;
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            yield return null;
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo still;
            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            still = new PreviewTileInfo(Vector2Int.zero, Grid.DirectionEnum.None, false);

            firstSteps.Add(still);
            return firstSteps;
        }
    }
}
