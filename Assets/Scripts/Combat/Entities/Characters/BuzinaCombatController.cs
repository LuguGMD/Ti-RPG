using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class BuzinaCombatController : CharacterController
    {
        [SerializeField] private BuzinaDriveAttack _driveAttack;

        protected override void InitCombatActions()
        {
            _actions.Add(_driveAttack);

            base.InitCombatActions();
        }
    }
}
