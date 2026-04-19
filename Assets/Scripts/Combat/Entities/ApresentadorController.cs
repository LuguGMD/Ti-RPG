using RPG.Combat.Actions;
using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat
{
    public class ApresentadorController : EntityController
    {
        [SerializeField] protected EntityScriptable _info;
        private float _motivationPoint;

        #region Properties

        public EntityScriptable Info { get { return _info; } }
        public float CurrentMotivation { get { return _motivationPoint; } }

        #endregion

        private void Start()
        {
            MapManager.Instance.AddTileObject(_tileObject, Map.CENTER_POS);
        }

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        public override void TakeDamage(float damage)
        {
            _motivationPoint -= damage;
            ActionsManager.Instance.OnApresentadorDamageTaken?.Invoke();

            Debug.Log(Info.name + " took " + damage + " damage\n Current Motivation Bar: " + _motivationPoint);
        }
    }
}
