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

        private void OnEnable()
        {
            ActionsManager.Instance.OnApresentadorDamageTaken += CheckDefeated;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorDamageTaken -= CheckDefeated;
        }

        public override void TakeDamage(float damage)
        {
            _motivation -= damage;

            CheckDefeated();

            Debug.Log(Info.name + " lost " + damage + " motivation\n Current Motivation: " + _motivation);
        }

        private void CheckDefeated()
        {
            if (CombatManager.Apresentador.CurrentMotivation < _motivation)
            {
                Defeated();
            }
        }

        protected override void Defeated()
        {
            CombatManager.Instance.RemoveCharacter(this);
            _tileObject.CurrentTile.SetTileObject(null);

            //TO DO triggar animacao de morte
            Destroy(gameObject);
        }

        protected override void OnSelected()
        {
            base.OnSelected();
            ActionsManager.Instance.OnCharacterSelected?.Invoke(this);
        }
    }
}
