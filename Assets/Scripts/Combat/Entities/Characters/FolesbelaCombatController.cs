using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class FolesbelaCombatController : CharacterController
    {
        //[SerializeField] private DonLiponWeightAttack _weightAttack;

        protected override void InitCombatActions()
        {
            //_actions.Add(_weightAttack);

            base.InitCombatActions();
        }
    }
}
