using Lugu.Singleton;
using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatFactory : SingletonMono<CombatFactory>
    {
        public static TileObject InstantiateTileObject(TileObject entityPrefab, Vector2Int startPosition)
        {
            TileObject characterObject = Instantiate<TileObject>(entityPrefab);
            MapManager.Instance.AddTileObject(characterObject, startPosition);
            characterObject.UpdatePosition();

            return characterObject;
        }
    }
}
