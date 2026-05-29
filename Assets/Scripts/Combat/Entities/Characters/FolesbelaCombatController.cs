using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class FolesbelaCombatController : CharacterController
    {
        [SerializeField] private FolesbelaAcrobaticsAttack _acrobaticsAttack;

        protected override void InitCombatActions()
        {
            _actions.Add(_acrobaticsAttack);

            base.InitCombatActions();
        }
    }
}
