using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class FolesbelaCombatController : CharacterController
    {
        [SerializeField] private FolesbelaAcrobaticsAttack _acrobaticsLeftAttack;
        [SerializeField] private FolesbelaAcrobaticsAttack _acrobaticsRightAttack;

        protected override void InitCombatActions()
        {
            _actions.Add(_acrobaticsLeftAttack);
            _actions.Add(_acrobaticsRightAttack);

            base.InitCombatActions();
        }
    }
}
