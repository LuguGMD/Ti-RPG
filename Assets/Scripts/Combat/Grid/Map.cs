using RPG.Extensions;
using RPG.Management.Progression;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Grid
{
    [System.Serializable]
    public class Map
    {
        private Dictionary<Vector2Int, Tile> _grid = new Dictionary<Vector2Int, Tile>();
        private static int _rows;
        private static int _columns;

        public static readonly Vector2Int CENTER_POS = new Vector2Int(0,-1);

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

            AddTile(CENTER_POS);
        }

        private void AddTile(Vector2Int position)
        {
            Transform tileTransform = new GameObject("TileTransform").transform;
            Tile tile = new Tile(position, tileTransform);
            _grid.Add(position, tile);

            //TO DO adicionar uma logica de posicionamento de upgrade depois

            bool isInUpgradeArea = false;
            if (GameManager.Instance != null)
            {
                switch (GameManager.CurrentTileEqquiped)
                {
                    case UpgradeConstants.UpgradeKey.TileDamageIncrease1:
                    case UpgradeConstants.UpgradeKey.TileDamageReduction1:
                    case UpgradeConstants.UpgradeKey.TilePushBlock1:
                        isInUpgradeArea = position.x == 0;
                        break;
                    case UpgradeConstants.UpgradeKey.TileDamageIncrease2:
                    case UpgradeConstants.UpgradeKey.TileDamageReduction2:
                    case UpgradeConstants.UpgradeKey.TilePushBlock2:
                        isInUpgradeArea = position.x == 0 || position.x == 1 || position.x == 11;
                        break;
                }

                if (isInUpgradeArea && position.y < Map.Rows - 1)
                    tile.SetUpgrade(GameManager.CurrentTileEqquiped);
            }

            tileTransform.position = MapManager.Instance.GetWorldPosition(position);
            if(position.y < MapManager.RowGameObjects.Length && position.y >= 0)
            {
                tileTransform.parent = MapManager.RowGameObjects[position.y].transform;
            }
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

            if (!_grid.ContainsKey(position)) return null;

            return _grid[position];
        }

        public Tile GetNeighborTile(Tile tile, DirectionEnum direction)
        {
            return GetTile(tile.Position + direction.ToVector2Int());
        }

        public Tile GetNeighborTile(Vector2Int tile, DirectionEnum direction)
        {
            return GetTile(tile + direction.ToVector2Int());
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
