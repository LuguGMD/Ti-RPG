using RPG.Combat.Wave;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Grid
{
    //This script is temporary and just for test purposes
    public class MapTest : MonoBehaviour
    {
        [SerializeField] private List<SpawnInfo> _characterSpawns;

        [SerializeField] private GameObject _apresentador;

        [SerializeField] private int _rowToRotate = 0;
        [SerializeField] private int _rotateAmount = 1;

        private void Start()
        {
            for (int i = 0; i < _characterSpawns.Count; i++)
            {
                CombatFactory.InstantiateCharacter(_characterSpawns[i]);
            }

            _apresentador.SetActive(true);
        }


        [ContextMenu("Rotate")]
        private void RotateTest()
        {
            MapManager.Instance.Map.RotateRow(_rowToRotate, _rotateAmount);
        }

        public void Rotate(int amount)
        {
            MapManager.Instance.Map.RotateRow(_rowToRotate, amount);
        }

        public void ChangeRow(int amount)
        {
            _rowToRotate += amount;
            _rowToRotate %= (Map.Rows);
        }

    }
}
