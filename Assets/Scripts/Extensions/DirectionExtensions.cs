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

        public static Direction Mirror(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.UpRight:
                    return Direction.DownLeft;
                case Direction.UpLeft:
                    return Direction.DownRight;
                case Direction.DownRight:
                    return Direction.UpLeft;
                case Direction.DownLeft:
                    return Direction.UpRight;
            }

            return Direction.Up;
        }
    }
}
