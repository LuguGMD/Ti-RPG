using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class MovementPattern 
    {
        [SerializeField] private List<Movement> _pattern = new List<Movement>();
        [Tooltip("How many times the pattern is repeated")]
        [SerializeField] private int _repetition = 1;
        [Tooltip("If the pattern can me used mirrored")]
        [SerializeField] private bool _canMirror = true;
    }
}
