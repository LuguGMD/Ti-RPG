using RPG.Combat.Preview;
using UnityEngine;

namespace RPG.Combat
{
    public class MediumFearTankEnemyController : EnemyController
    {
        public override void PrepareAction()
        {
            if(CombatManager.TurnCount % 2 == 0)
            {
                SelectAction(0);
            }
            else
            {
                SelectAction(1);
            }

            _preparedAction = new PreviewTileInfo(Vector2Int.zero, Grid.DirectionEnum.None, false);
        }
    }
}
