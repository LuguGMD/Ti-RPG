using UnityEngine;

namespace RPG.Combat
{
    public class EnemyController : StageEntityController
    {
        private float _health = 5;

        public override void TakeDamage(float damage)
        {
            _health -= damage;

            Debug.Log(Info.name + " lost " + damage + " health\n Current Health: " + _health);
        }
    }
}
