using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{

    public class CharacterController : StageEntityController
    {
        private CharacterScriptable _characterInfo;
        private float _currentMotivation;

        #region Properties

        public float CurrentMotivation
        {
            get { return _currentMotivation; }
        }

        #endregion

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected new void Start()
        {
            base.Start();
            Initialize();
            ActionsManager.Instance.OnCharacterCreated?.Invoke(this);
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            ActionsManager.Instance.OnApresentadorDamageTaken += CheckDefeated;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            ActionsManager.Instance.OnApresentadorDamageTaken -= CheckDefeated;
        }

        private void Initialize()
        {
            _characterInfo = (CharacterScriptable)_info;
            _currentMotivation = _characterInfo.Motivation;
        }

        #region Health

        public override void TakeDamage(float damage)
        {
            _currentMotivation -= damage;

            ActionsManager.Instance.OnCharacterDamageTaken?.Invoke(this);

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

            ActionsManager.Instance.OnCharacterDefeated?.Invoke(this);

            //TO DO triggar animacao de morte
            Destroy(gameObject);
        }

        #endregion

        protected override void OnSelected()
        {
            base.OnSelected();
            ActionsManager.Instance.OnCharacterSelected?.Invoke(this);
        }
    }
}
