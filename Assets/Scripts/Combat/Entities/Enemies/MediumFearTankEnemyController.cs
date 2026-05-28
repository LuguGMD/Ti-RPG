using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class MediumFearTankEnemyController : EnemyController
    {
        [SerializeField] private BasicFearAttack _attack;
        private EnemyNothingAction _nothing = new EnemyNothingAction();

        protected override void InitCombatActions()
        {
            _actions.Add(_attack);
            _actions.Add(_nothing);

            base.InitCombatActions();
        }

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

            List<PreviewTileInfo> tiles = _attack.Preview();
            _preparedAction = tiles[0];
        }
    }
}
