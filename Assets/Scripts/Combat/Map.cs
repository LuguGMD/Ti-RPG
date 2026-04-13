using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable]
    public class Map
    {
        private Dictionary<Vector2Int, Tile> _grid = new Dictionary<Vector2Int, Tile>();
        private static int _rows;
        private static int _columns;

        #region Properties

        public static int Rows { get { return _rows; } }
        public static int Columns { get { return _columns; } }

        #endregion

        public Map(int rows, int colums)
        {
            _rows = rows;
            _columns = colums;
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    AddTile(new Vector2Int(i, j));
                }
            }
        }

        private void AddTile(Vector2Int position)
        {
            _grid.Add(position, new Tile(position));
        }

        private void SetTile(Vector2Int position, Tile tile)
        {
            position = position.ClampMap();
            _grid[position] = tile;

            tile.SetPosition(position);
        }

        public Tile GetTile(Vector2Int position)
        {
            position = position.ClampMap();

            return _grid[position];
        }

        public void RotateRow(int rowIndex, int amount)
        {
            Tile[] row = new Tile[_columns];

            for (int i = 0; i < _columns; ++i)
            {
                row[i] = GetTile(new Vector2Int(i, rowIndex));
            }

            for (int i = 0; i < _columns; i++)
            {
                int newPos = i + amount;
                SetTile(new Vector2Int(newPos, rowIndex), row[i]);
            }

            ActionsManager.Instance.OnMapChanged?.Invoke();
        }
    }
}
