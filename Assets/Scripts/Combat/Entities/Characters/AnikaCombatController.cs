using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class AnikaCombatController : CharacterController
    {
        [SerializeField] private AnikaJumpAttack _jumpAttack;

        protected override void InitCombatActions()
        {
            _actions.Add(_jumpAttack);

            base.InitCombatActions();
        }
    }
}
