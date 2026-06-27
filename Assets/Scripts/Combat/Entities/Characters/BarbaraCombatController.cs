using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class BarbaraCombatController : CharacterController
    {
        [SerializeField] private BarbaraSongAttack _songAttack;

        protected override void InitCombatActions()
        {
            _actions.Add(_songAttack);

            base.InitCombatActions();
        }
    }
}
