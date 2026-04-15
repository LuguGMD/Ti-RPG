using System;
using UnityEngine;

namespace RPG.Combat.Grid
{
    //This script is temporary and just for test purposes
    public class MapTest : MonoBehaviour
    {
        [SerializeField] private TileObject _characterPrefab;
        [SerializeField] private TileObject _enemyPrefab;

        [SerializeField] private int _rowToRotate = 0;
        [SerializeField] private int _rotateAmount = 1;

        private void Start()
        {
            TileObject characterObject = Instantiate<TileObject>(_characterPrefab);
            TileObject enemyObject = Instantiate<TileObject>(_enemyPrefab);

            characterObject.gameObject.SetActive(true);
            enemyObject.gameObject.SetActive(true);

            MapManager.Instance.AddTileObject(characterObject, new Vector2Int(0, 0));
            MapManager.Instance.AddTileObject(enemyObject, new Vector2Int(0, 2));

            characterObject.UpdatePosition();
            enemyObject.UpdatePosition();
        }



        [ContextMenu("Rotate")]
        private void RotateTest()
        {
            MapManager.Instance.Map.RotateRow(_rowToRotate, _rotateAmount);
        }

    }
}
