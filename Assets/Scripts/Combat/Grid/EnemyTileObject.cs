using UnityEngine;

namespace RPG.Combat.Grid
{
    public class EnemyTileObject : TileObject
    {
        public override void UpdatePosition()
        {
            base.UpdatePosition();

            transform.LookAt(Vector3.zero);
        }
    }
}
