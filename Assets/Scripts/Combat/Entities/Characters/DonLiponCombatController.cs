using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class DonLiponCombatController : CharacterController
    {
        [SerializeField] private DonLiponWeightAttack _weightAttack;

        protected override void InitCombatActions()
        {
            _actions.Add(_weightAttack);

            base.InitCombatActions();
        }
    }
}
