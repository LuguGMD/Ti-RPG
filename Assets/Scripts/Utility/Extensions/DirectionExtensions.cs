using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Extensions
{
    public static class DirectionExtensions
    {
        public static Vector2Int ToVector2Int(this DirectionEnum direction)
        {
            switch(direction)
            {
                case DirectionEnum.Left:
                    return new Vector2Int(-1, 0);
                    case DirectionEnum.Right:
                    return new Vector2Int(1, 0);
                case DirectionEnum.Up:
                    return new Vector2Int(0, 1);
                case DirectionEnum.Down:
                    return new Vector2Int(0, -1);
                case DirectionEnum.UpRight:
                    return new Vector2Int(1, 1);
                case DirectionEnum.UpLeft:
                    return new Vector2Int(-1, 1);
                case DirectionEnum.DownRight:
                    return new Vector2Int(1, -1);
                case DirectionEnum.DownLeft:
                    return new Vector2Int(-1, -1);
            }

            return Vector2Int.zero;
        }

        public static DirectionEnum Mirror(this DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Left:
                    return DirectionEnum.Right;
                case DirectionEnum.Right:
                    return DirectionEnum.Left;
                case DirectionEnum.Up:
                    return DirectionEnum.Down;
                case DirectionEnum.Down:
                    return DirectionEnum.Up;
                case DirectionEnum.UpRight:
                    return DirectionEnum.DownLeft;
                case DirectionEnum.UpLeft:
                    return DirectionEnum.DownRight;
                case DirectionEnum.DownRight:
                    return DirectionEnum.UpLeft;
                case DirectionEnum.DownLeft:
                    return DirectionEnum.UpRight;
            }

            return DirectionEnum.None;
        }

        public static DirectionEnum RelativeTo(this DirectionEnum direction, DirectionEnum relative)
        {
            switch(relative)
            {
                case DirectionEnum.Up:
                    switch(direction)
                    {
                        case DirectionEnum.Up:
                            return DirectionEnum.Up;
                        case DirectionEnum.Down:
                            return DirectionEnum.Down;
                        case DirectionEnum.Right:
                            return DirectionEnum.Right;
                        case DirectionEnum.Left:
                            return DirectionEnum.Left;
                    }
                    break;
                case DirectionEnum.Down:
                    switch (direction)
                    {
                        case DirectionEnum.Up:
                            return DirectionEnum.Down;
                        case DirectionEnum.Down:
                            return DirectionEnum.Up;
                        case DirectionEnum.Right:
                            return DirectionEnum.Left;
                        case DirectionEnum.Left:
                            return DirectionEnum.Right;
                    }
                    break;
                case DirectionEnum.Right:
                    switch (direction)
                    {
                        case DirectionEnum.Up:
                            return DirectionEnum.Right;
                        case DirectionEnum.Down:
                            return DirectionEnum.Left;
                        case DirectionEnum.Right:
                            return DirectionEnum.Down;
                        case DirectionEnum.Left:
                            return DirectionEnum.Up;
                    }
                    break;
                case DirectionEnum.Left:
                    switch (direction)
                    {
                        case DirectionEnum.Up:
                            return DirectionEnum.Left;
                        case DirectionEnum.Down:
                            return DirectionEnum.Right;
                        case DirectionEnum.Right:
                            return DirectionEnum.Up;
                        case DirectionEnum.Left:
                            return DirectionEnum.Down;
                    }
                    break;
            }

            return direction;
        }

        public static bool IsSideways(this DirectionEnum direction)
        {
            return direction == DirectionEnum.Left || direction == DirectionEnum.Right;
        }
    }
}
