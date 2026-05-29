using RPG.Combat.Grid;
using RPG.Extensions;
using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    public class PushRelativeEffect : EffectCommand
    {
        private int _pushAmount = 1;

        #region Properties
        public int PushAmount { get { return _pushAmount; } }

        #endregion

        public PushRelativeEffect(int pushAmount)
        {
            _pushAmount = pushAmount;
        }

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            DirectionEnum pushDirection = (target.Position - user.Position).ToDirection();

            Movement movement = new Movement(pushDirection, true);

            if (!MapManager.IsMovementValid(target.Position, movement))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < _pushAmount; i++)
                {
                    target.Movement.Push(movement);
                }
            }

            return true;
        }

        public override bool ExecuteApresentador(StageEntityController user, ApresentadorController target)
        {
            return false;
        }
    }
}
