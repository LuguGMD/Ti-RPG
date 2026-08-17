using RPG.Combat.Grid;
using RPG.Extensions;
using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [System.Serializable]
    public class PushEffect : EffectCommand
    {
        private bool _isPushDirectionRelative = true;
        private DirectionEnum _pushDirection = DirectionEnum.Up;
        private int _pushAmount = 1;

        #region Properties

        public bool IsPushDirectionRelative { get { return _isPushDirectionRelative; } }
        public DirectionEnum PushDirection { get { return _pushDirection; } }
        public int PushAmount { get { return _pushAmount; } }

        #endregion

        public PushEffect(DirectionEnum pushDirection, int pushAmount, bool isPushDirectionRelative = true)
        {
            _isPushDirectionRelative |= isPushDirectionRelative;
            _pushDirection = pushDirection;
            _pushAmount = pushAmount;
        }

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            DirectionEnum facing = user.Direction;
            DirectionEnum pushDirection = _isPushDirectionRelative ? _pushDirection.RelativeTo(facing) : _pushDirection;

            Movement movement = new Movement(pushDirection, true);

            if (!MapManager.IsMovementValid(target.Position, movement, target.Movement.CanGoToLastRow))
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
