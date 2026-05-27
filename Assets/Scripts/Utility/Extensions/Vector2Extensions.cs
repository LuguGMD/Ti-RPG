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
    }
}
