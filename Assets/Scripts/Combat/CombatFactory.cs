using Lugu.Singleton;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using RPG.Combat.Wave;
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

        public static TileObject InstantiateTileObject(SpawnInfo spawnInfo)
        {
            return InstantiateTileObject(spawnInfo.TileObjectPrefab, spawnInfo.SpawnPosition);
        }

        public static EnemyController InstantiateEnemy(EnemySpawnInfo spawnInfo)
        {
            EnemyController enemyInstance = Instantiate<EnemyController>(spawnInfo.EnemyInfo.Prefab);

            TileObject enemyObject = enemyInstance.GetComponent<TileObject>();
            MapManager.Instance.AddTileObject(enemyObject, new Vector2Int(spawnInfo.SpawnPosition, Map.Rows-1));
            enemyObject.UpdatePosition();

            CombatManager.Instance.AddEnemy(enemyInstance);
            return enemyInstance;
        }

        public static CharacterController InstantiateCharacter(SpawnInfo spawnInfo)
        {
            TileObject tileObject = InstantiateTileObject(spawnInfo);

            //TO DO adicionar spawn info especifico para character depois
            CharacterController character = tileObject.GetComponent<CharacterController>();
            CombatManager.Instance.AddCharacter(character);

            return character;
        }

        public static PreviewTile InstantiatePreviewTile()
        {
            PreviewTile prefab = CombatManager.PreviewTilePrefab;
            PreviewTile instance = Instantiate(prefab);
            return instance;
        }
    }
}
