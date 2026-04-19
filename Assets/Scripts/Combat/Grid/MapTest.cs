using RPG.Combat.Wave;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Grid
{
    //This script is temporary and just for test purposes
    public class MapTest : MonoBehaviour
    {
        [SerializeField] private List<CharacterSpawnInfo> _characterSpawns;

        [SerializeField] private GameObject _apresentador;

        private void Start()
        {
            for (int i = 0; i < _characterSpawns.Count; i++)
            {
                CombatFactory.InstantiateCharacter(_characterSpawns[i]);
            }
        }

    }
}
