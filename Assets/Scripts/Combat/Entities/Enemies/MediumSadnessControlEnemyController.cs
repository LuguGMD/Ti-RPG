using RPG.Combat.Actions;
using RPG.Combat.Preview;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class MediumSadnessControlEnemyController : EnemyController
    {
        [SerializeField] private MediumSadnessRegularAttack _regularAttack;

        protected override void InitCombatActions()
        {
            if (_actions.Count == 0)
            {
                _actions.Add(_regularAttack);
                base.InitCombatActions();
            }
        }

        public override void PrepareAction()
        {
            if (_actions.Count == 0)
            {
                InitCombatActions();
            }

            SelectAction(0);

            List<PreviewTileInfo> tiles = _regularAttack.Preview();
            _preparedAction = tiles[0];
        }
    }
}
