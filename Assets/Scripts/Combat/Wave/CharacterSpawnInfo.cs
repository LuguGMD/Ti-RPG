using UnityEngine;

namespace RPG.Combat.Wave
{
    [System.Serializable]
    public class CharacterSpawnInfo
    {
        [SerializeField] private CharacterScriptable _characterInfo;
        [SerializeField] private Vector2Int _spawnPosition;

        #region Properties

        public CharacterScriptable CharacterInfo { get { return _characterInfo; } }
        public Vector2Int SpawnPosition { get { return _spawnPosition; } }

        #endregion

        public CharacterSpawnInfo(CharacterScriptable characterInfo, Vector2Int spawnPosition)
        {
            _characterInfo = characterInfo;
            _spawnPosition = spawnPosition;
        }
    }
}
