using RPG.Combat.Grid;
using RPG.Extensions;
using UnityEngine;

namespace RPG.Combat.Actions.Effects
{
    [CreateAssetMenu(fileName = "PushEffectScriptable", menuName = "Scriptable Objects/Effects/Push")]
    public class PushEffectScriptable : EffectCommandScriptable
    {
        [SerializeField] private bool _isPushDirectionRelative = true;
        [SerializeField] private Direction _pushDirection = Direction.Up;
        [Min(1)] [SerializeField] private int _pushAmount = 1;

        #region Properties

        public bool IsPushDirectionRelative { get { return _isPushDirectionRelative; } }
        public Direction PushDirection { get { return _pushDirection; } }
        public int PushAmount { get { return _pushAmount; } }

        #endregion

        public override bool Execute(StageEntityController user, StageEntityController target)
        {
            Direction facing = user.Direction;
            Direction pushDirection = _isPushDirectionRelative ? _pushDirection.RelativeTo(facing) : _pushDirection;

            Movement movement = new Movement(pushDirection, true);

            if (!MapManager.IsMovementValid(target.Position, movement, false))
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
