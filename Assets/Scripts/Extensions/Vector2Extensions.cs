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
            vector.y += Map.Rows;

            vector.x %= Map.Columns;
            vector.y %= Map.Rows;

            return vector;
        }
    }
}
