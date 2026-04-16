using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public class ApresentadorController : EntityController
    {
        [SerializeField] protected EntityScriptable _info;
        private float _motivationPoint;

        #region Properties

        public EntityScriptable Info { get { return _info; } }

        #endregion

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        public override void TakeDamage(float damage)
        {
            _motivationPoint -= damage;

            Debug.Log(Info.name + " took " + damage + " damage\n Current Motivation Bar: " + _motivationPoint);
        }
    }
}
