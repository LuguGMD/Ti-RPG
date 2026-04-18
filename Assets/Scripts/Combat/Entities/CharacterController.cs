using UnityEngine;

namespace RPG.Combat
{
    
    public class CharacterController : StageEntityController
    {
        
        private float _motivation = 10;

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        public override void TakeDamage(float damage)
        {
            _motivation -= damage;

            Debug.Log(Info.name + " lost " + damage + " motivation\n Current Motivation: " + _motivation);
        }
    }
}
