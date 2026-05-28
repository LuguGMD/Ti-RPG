using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class BasicFearEnemyController : EnemyController
    {
        [SerializeField] private BasicFearAttack _attack;
        protected override void InitCombatActions()
        {
            _actions.Add(_attack);

            base.InitCombatActions();
        }

        public override void PrepareAction()
        {
            List<PreviewTileInfo> tiles = _attack.Preview();
            _preparedAction = tiles[0];
        }

    }
}
