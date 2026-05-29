using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class MovementPattern 
    {
        [SerializeField] private List<Movement> _pattern = new List<Movement>();
        [Tooltip("If the pattern can me used mirrored")]
        [SerializeField] private bool _canMirror = true;

        #region Properties

        public List<Movement> Pattern { get { return _pattern; } }
        public bool CanMirror { get { return _canMirror; } }

        #endregion
    }
}
