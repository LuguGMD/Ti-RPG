using Lugu.Singleton;
using UnityEngine;
using UnityEngine.Splines;

namespace RPG.Combat.Grid
{
    public class MapManager : SingletonMono<MapManager>
    {
        private Map _map;

        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private SplineContainer _mapSplineContainer;

        [SerializeField] private float _rowRadius = 2f;
        [SerializeField] private float _centerOffset = 3f;

        #region Properties

        public Map Map
        {
            get { return _map; }
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            if (Instance == this)
            {
                _map = new Map(_rows, _columns);
            }
        }


        [ContextMenu("Generate Map")]
        private void GenerateMap()
        {
            while(_mapSplineContainer.Splines.Count > 0)
            { 
                _mapSplineContainer.RemoveSplineAt(0);
            }

            for (int i = 1; i <= _rows; i++)
            {
                Spline spline = SplineFactory.CreateCircle(_centerOffset + _rowRadius * i);
                _mapSplineContainer.AddSpline(spline);
            }
        }

        public void AddTileObject(TileObject tileObject, Vector2Int position)
        {
            Tile tile = _map.GetTile(position);

            tileObject.SetCurrentTile(tile);
            tile.SetTileObject(tileObject);
        }

        public bool IsPositionOccupied(Vector2Int position)
        {
            return _map.GetTile(position).IsOccupied;
        }

        public Vector3 GetWorldPostion(Vector2Int tilePosition)
        {
            Spline spline = _mapSplineContainer[tilePosition.y];
            float percentage = (float)tilePosition.x / (float)Map.Columns;
            Vector3 worldPosition = spline.EvaluatePosition(percentage);

            return worldPosition;
        }

        

    }
}
