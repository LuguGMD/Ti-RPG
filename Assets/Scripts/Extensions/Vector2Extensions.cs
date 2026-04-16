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
    }
}
