using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyController : StageEntityController
    {
        private float _health = 5;

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        public override void TakeDamage(float damage)
        {
            _health -= damage;

            if(_health <= 0)
            {
                Defeated();
            }

            Debug.Log(Info.name + " lost " + damage + " health\n Current Health: " + _health);
        }

        protected override void Defeated()
        {
            CombatManager.Instance.RemoveEnemy(this);
            _tileObject.CurrentTile.SetTileObject(null);
            //TO DO triggar animacao de morte
            Destroy(gameObject);
        }
    }
}
