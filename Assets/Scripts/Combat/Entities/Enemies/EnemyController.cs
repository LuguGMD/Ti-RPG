using RPG.Combat.Grid;
using RPG.Combat.Preview;
using RPG.Combat.UI;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(EnemyHealthBar))]
    public class EnemyController : StageEntityController
    {
        protected EnemyScriptable _enemyInfo;

        private EnemyHealthBar _healthBar;
        private float _health;
        protected PreviewTileInfo _preparedAction;

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected new void Start()
        {
            base.Start();
            Initialize();
        }

        private void Initialize()
        {
            _enemyInfo = (EnemyScriptable)_info;
            _health = _enemyInfo.Health;

            _healthBar = GetComponent<EnemyHealthBar>();

            UpdateHealthBar();
        }

        public override void TakeDamage(float damage)
        {
            _health -= damage;

            CombatManager.Instance.CameraShake(0.1f);

            UpdateHealthBar();
            base.TakeDamage(damage);
        }

        public override void Heal(float heal)
        {
            _health += heal;
            _health = Mathf.Clamp(_health, 0, _enemyInfo.Health);

            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            _healthBar.Slider.value = (_health / _enemyInfo.Health);
        }

        protected override void CheckDefeated()
        {
            if (_health <= 0)
            {
                Defeated();
            }
        }
        protected override void Defeated()
        {
            base.Defeated();

            ActionsManager.Instance.OnEnemyDefeated?.Invoke(this);
            CombatManager.Instance.RemoveEnemy(this);
            _tileObject.CurrentTile.SetTileObject(null);
            //TO DO esperar fim animação de morte
            Destroy(gameObject);
        }

        public IEnumerator UsePreparedAction()
        {
            //TO DO guardar acao preparada e usar aqui
            yield return UseSelectedAction(_preparedAction);
        }

        public virtual void PrepareAction()
        {

        }
    }
}
