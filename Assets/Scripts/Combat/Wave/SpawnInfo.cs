using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat.Wave
{
    [System.Serializable]
    public class SpawnInfo
    {
        [SerializeField] private TileObject _tileObjectPrefab;
        [SerializeField] private Vector2Int _spawnPosition;

        #region Properties

        public TileObject TileObjectPrefab { get { return _tileObjectPrefab; } }
        public Vector2Int SpawnPosition { get { return _spawnPosition; } }

        #endregion

        public SpawnInfo(TileObject tileObjectprefab, Vector2Int spawnPosition)
        {
            _tileObjectPrefab = tileObjectprefab;
            _spawnPosition = spawnPosition;
        }
    }
}
