using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Extensions
{
    public static class DirectionExtensions
    {
        public static Vector2Int ToVector2Int(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Left:
                    return new Vector2Int(-1, 0);
                    case Direction.Right:
                    return new Vector2Int(1, 0);
                case Direction.Up:
                    return new Vector2Int(0, 1);
                case Direction.Down:
                    return new Vector2Int(0, -1);
                case Direction.UpRight:
                    return new Vector2Int(1, 1);
                case Direction.UpLeft:
                    return new Vector2Int(-1, 1);
                case Direction.DownRight:
                    return new Vector2Int(1, -1);
                case Direction.DownLeft:
                    return new Vector2Int(-1, -1);
            }

            return Vector2Int.zero;
        }
    }
}
