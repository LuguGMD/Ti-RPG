using RPG.Combat.Grid;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyController : StageEntityController
    {
        private float _health;

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected new void Start()
        {
            base.Start();
            EnemyScriptable info = (EnemyScriptable)_info;
            _health = info.Health;
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

        public IEnumerator UsePreparedAction()
        {
            //TO DO guardar acao preparada e usar aqui
            yield return UseAction(0, 0, 1, false);
        }
    }
}
