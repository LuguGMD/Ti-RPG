using UnityEngine;

namespace RPG.Combat.Grid
{
    public class EnemyTileObject : TileObject
    {
        protected new void Awake()
        {
            base.Awake();

            SetDirection(Direction.Down);
        }

        public override void UpdatePosition()
        {
            base.UpdatePosition();
            transform.LookAt(Vector3.zero);
        }
    }
}
