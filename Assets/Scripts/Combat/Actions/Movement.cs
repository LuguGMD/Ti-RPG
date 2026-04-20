using RPG.Combat.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class Movement
    {
        [SerializeField] private DirectionEnum _direction;
        [Tooltip("If the destination tile needs to be empty")]
        [SerializeField] private bool _needsToBeEmpty = true;

        #region Properties

        public DirectionEnum Direction
        {
            get { return _direction; }
        }

        public bool NeedsToBeEmpty { get { return _needsToBeEmpty; } }

        #endregion

        public Movement() { }
        public Movement(DirectionEnum direction, bool needsToBeEmpty)
        {
            _direction = direction;
            _needsToBeEmpty = needsToBeEmpty;
        }
    }
}
