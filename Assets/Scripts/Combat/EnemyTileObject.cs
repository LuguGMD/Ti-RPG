using UnityEngine;

namespace RPG.Combat
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
