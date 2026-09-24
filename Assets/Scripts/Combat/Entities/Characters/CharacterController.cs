using RPG.Combat.Actions;
using RPG.Combat.Preview;
using RPG.Management.Progression;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{

    public class CharacterController : StageEntityController
    {
        private CharacterScriptable _characterInfo;
        private float _currentMotivation;
        [SerializeField] private CharacterDirectionalPunch _characterDirectionalPunch;

        #region Properties

        public CharacterScriptable CharacterInfo { get { return _characterInfo; } }
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
            Movement.CanGoToLastRow = false;
        }

        private void Update()
        {
            if (!UnityEngine.Input.GetKey(KeyCode.LeftShift)) return;

            if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha1) || UnityEngine.Input.GetKeyUp(KeyCode.Alpha2) || UnityEngine.Input.GetKeyUp(KeyCode.Alpha3) || UnityEngine.Input.GetKeyUp(KeyCode.Alpha4) || UnityEngine.Input.GetKeyUp(KeyCode.Alpha5))
            {
                if (GameManager.CurrentParty[CombatManager.PlaceIndex] == _info)
                {
                    Defeated();
                }
            }
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
            float motivation = _characterInfo.Motivation;
            _currentMotivation = motivation;
        }

        #region Health

        public override void TakeDamage(float damage)
        {
            if (_tileObject.CurrentTile.TileUpgrade == UpgradeConstants.UpgradeKey.TileDamageIncrease1 ||
            _tileObject.CurrentTile.TileUpgrade == UpgradeConstants.UpgradeKey.TileDamageIncrease1) damage *= 1.5f;

            if (_tileObject.CurrentTile.TileUpgrade == UpgradeConstants.UpgradeKey.TileDamageReduction1 ||
            _tileObject.CurrentTile.TileUpgrade == UpgradeConstants.UpgradeKey.TileDamageReduction1) damage /= 1.5f;

            _currentMotivation -= damage;
            _currentMotivation = Mathf.Clamp(_currentMotivation, 0, CombatConstants.MAX_MOTIVATION_APRESENTADOR);

            ActionsManager.Instance.OnCharacterDamageTaken?.Invoke(this);

            base.TakeDamage(damage);
        }

        public override void Heal(float heal)
        {
            _currentMotivation += heal;
            _currentMotivation = Mathf.Clamp(_currentMotivation, 0, CombatConstants.MAX_MOTIVATION_APRESENTADOR);

            ActionsManager.Instance.OnCharacterHealed?.Invoke(this);
        }

        protected override void CheckDefeated()
        {
            if (CombatManager.Apresentador.CurrentMotivation <= CombatConstants.MAX_MOTIVATION_APRESENTADOR - _currentMotivation)
            {
                Defeated();
            }
        }

        protected override void Defeated()
        {
            base.Defeated();

            CombatManager.Instance.RemoveCharacter(this);
            _tileObject.CurrentTile.SetTileObject(null);

            ActionsManager.Instance.OnCharacterDefeated?.Invoke(this);

            //TO DO triggar animacao de morte
            Destroy(gameObject);
        }

        #endregion

        protected override void CheckHovered(Vector2Int hoveredPositon)
        {
            if (!_isHovered && hoveredPositon == Position)
            {
                _isHovered = true;
                ActionsManager.Instance.OnCharacterHoverEnter?.Invoke(this);
            }
            else if (_isHovered && hoveredPositon != Position)
            {
                _isHovered = false;
                ActionsManager.Instance.OnCharacterHoverExit?.Invoke(this);
            }
        }

        protected override void OnHoverStart()
        {
            base.OnHoverStart();
            ActionsManager.Instance.OnCharacterHoverEnter?.Invoke(this);
        }

        protected override void OnHoverEnd()
        {
            base.OnHoverEnd();
            ActionsManager.Instance.OnCharacterHoverExit?.Invoke(this);
        }

        protected override void OnSelected()
        {
            if (!CombatManager.HasCombatStarted) return;
            base.OnSelected();
            ActionsManager.Instance.OnCharacterClicked?.Invoke(this);
        }

        protected override void InitCombatActions()
        {
            _actions.Add(_characterDirectionalPunch);

            base.InitCombatActions();
        }

        public override void ResetAction()
        {
            base.ResetAction();
            ActionsManager.Instance.OnCharacterActionReset?.Invoke(this);
        }

        public override IEnumerator UseSelectedAction(PreviewTileInfo selectedPreviewTile)
        {
            ActionsManager.Instance.OnCharacterActionUsed?.Invoke(this);
            return base.UseSelectedAction(selectedPreviewTile);
        }
    }
}
