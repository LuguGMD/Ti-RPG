using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class BasicSadnessEnemyController : EnemyController
    {
        [SerializeField] private BasicSadnessAttack _attack;

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
