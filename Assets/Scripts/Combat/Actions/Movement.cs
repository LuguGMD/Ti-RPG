using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class Movement
    {
        [SerializeField] private Direction _direction;
        [Tooltip("If the destination tile needs to be empty")]
        [SerializeField] private bool _needsToBeEmpty;
    }
}
