using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class MediumFearTankEnemyController : EnemyController
    {
        [SerializeField] private BasicFearAttack _attack;
        [SerializeField] private EnemyNothingAction _nothing;

        protected override void InitCombatActions()
        {
            if (_actions.Count == 0)
            {
                _actions.Add(_attack);
                _actions.Add(_nothing);

                base.InitCombatActions();
            }
        }

        public override void PrepareAction()
        {
            if(_actions.Count == 0)
            {
                InitCombatActions();
            }

            if (CombatManager.TurnCount % 2 == 0)
            {
                SelectAction(1);
            }
            else
            {
                SelectAction(0);
            }

            List<PreviewTileInfo> tiles = _attack.Preview();
            _preparedAction = tiles[0];
        }
    }
}
