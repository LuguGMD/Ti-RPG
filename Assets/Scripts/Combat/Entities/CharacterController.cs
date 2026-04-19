using UnityEngine;

namespace RPG.Combat
{
    
    public class CharacterController : StageEntityController
    {
        
        private float _currentMotivation;

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected new void Start()
        {
            base.Start();
            CharacterScriptable info = (CharacterScriptable)_info;
            _currentMotivation = info.Motivation;
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
            _currentMotivation -= damage;

            CheckDefeated();

            Debug.Log(Info.name + " lost " + damage + " motivation\n Current Motivation: " + _currentMotivation);
        }

        private void CheckDefeated()
        {
            if (CombatManager.Apresentador.CurrentMotivation < CombatConstants.MAX_MOTIVATION_APRESENTADOR - _currentMotivation)
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
