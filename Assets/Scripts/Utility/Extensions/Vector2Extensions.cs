using RPG.Combat.Grid;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2Int ClampMap(this Vector2Int vector)
        {
            vector.x += Map.Columns;
            vector.x %= Map.Columns;

            //vector.y += Map.Rows;
            //vector.y %= Map.Rows;

            if (vector.y < 0) vector = Map.CENTER_POS;

            return vector;
        }

        public static Vector2Int RelativeTo(this Vector2Int vector, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Up:
                    return vector;
                case DirectionEnum.Down:
                    return -vector;
                case DirectionEnum.Right:
                    return new Vector2Int(vector.y, vector.x);
                case DirectionEnum.Left:
                    return new Vector2Int(-vector.y, -vector.x);
            }

            return vector;
        }

        public static DirectionEnum ToDirection(this Vector2Int vector)
        {
            if (vector == Vector2Int.left) return DirectionEnum.Left;
            if (vector == Vector2Int.right) return DirectionEnum.Right;
            if (vector == Vector2Int.up) return DirectionEnum.Up;
            if (vector == Vector2Int.down) return DirectionEnum.Down;
            if (vector == Vector2Int.up + Vector2Int.right) return DirectionEnum.UpRight;
            if (vector == Vector2Int.up + Vector2Int.left) return DirectionEnum.UpLeft;
            if (vector == Vector2Int.down + Vector2Int.right) return DirectionEnum.DownRight;
            if (vector == Vector2Int.down + Vector2Int.left) return DirectionEnum.DownLeft;

            return DirectionEnum.None;
        }
    }
}
