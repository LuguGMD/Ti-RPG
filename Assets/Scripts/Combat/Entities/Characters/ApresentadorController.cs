using FMODUnity;
using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class ApresentadorController : StageEntityController
    {
        private float _currentMotivation;

        private int _maxRowRotations = 3;
        private int _rowToRotate = 0;
        private int _superCharge = 0;
        private SuperHandler _equippedSuper;

        

        #region Properties

        public float CurrentMotivation { get { return _currentMotivation; } }
        public int RowToRotate { get { return _rowToRotate; } }
        public int MaxRowRotations { get { return _maxRowRotations; } }
        public int SuperCharge { get { return _superCharge; } }
        public SuperHandler EquippedSuper { get { return _equippedSuper; } }

        #endregion

        protected new void Start()
        {
            base.Start();
            Initialize();
        }

        protected new void OnEnable()
        {
            ActionsManager.Instance.OnCombatSpeedChanged += AdjsutGameSpeed;
            ActionsManager.Instance.OnPreviewTileSelected += CheckSelected;
            ActionsManager.Instance.OnEnemyTurnEnded += ResetAction;
        }

        protected new void OnDisable()
        {
            ActionsManager.Instance.OnCombatSpeedChanged -= AdjsutGameSpeed;
            ActionsManager.Instance.OnPreviewTileSelected -= CheckSelected;
            ActionsManager.Instance.OnEnemyTurnEnded -= ResetAction;
        }

        private void Initialize()
        {
            _currentMotivation = CombatConstants.MAX_MOTIVATION_APRESENTADOR;
            MapManager.Instance.AddTileObject(_tileObject, Map.CENTER_POS);
            AdjsutGameSpeed();

            //TO DO remover depois DEBUG
            EquipSuper(new SuperHealHandler());
        }

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected override void OnSelected()
        {
            if (!CombatManager.HasCombatStarted) return;
            if (CombatManager.IsActionInProgress) return;

            base.OnSelected();

            if (!_hasActed)
            {
                ActionsManager.Instance.OnApresentadorSelected?.Invoke();
            }
            else
            {
                //TO DO aviso que acao ja foi feita
            }
        }

        public override void TakeDamage(float damage)
        {
            _currentMotivation -= damage;
            _currentMotivation = Mathf.Clamp(_currentMotivation, 0, CombatConstants.MAX_MOTIVATION_APRESENTADOR);
            ActionsManager.Instance.OnApresentadorDamageTaken?.Invoke();

            base.TakeDamage(damage);
        }

        protected override void CheckDefeated()
        {
            
        }

        public override void Heal(float heal)
        {
            _currentMotivation += heal;
            _currentMotivation = Mathf.Clamp(_currentMotivation, 0, CombatConstants.MAX_MOTIVATION_APRESENTADOR);

            ActionsManager.Instance.OnApresentadorHealed?.Invoke();
        }

        public void Rotate(int amount)
        {
            MapManager.Instance.RotateRow(_rowToRotate, amount);
        }

        public void Rotate(int row, int amount)
        {
            MapManager.Instance.RotateRow(row, amount);
        }

        public void ChangeRow(int amount)
        {
            _rowToRotate += amount;

            _rowToRotate += (Map.Rows - 1);
            _rowToRotate %= (Map.Rows - 1);
        }

        #region Super

        public void ChargeSuper(int amount)
        {
            _superCharge += amount;
        }

        public bool UseSuper()
        {
            if(_superCharge >= _equippedSuper.ChargeAmount)
            {
                _superCharge = 0;

                List<PreviewTileInfo> _tiles = _equippedSuper.Preview();
                StartCoroutine(CombatManager.Instance.SuperActionCoroutine(_tiles[0]));
                return true;
            }

            return false;
        }

        public void EquipSuper(SuperHandler super)
        {
            _equippedSuper = super;
            _equippedSuper.Init(this);
        }

        #endregion

        public void CompleteAction()
        {
            _hasActed = true;
        }

        public override void ResetAction()
        {
            base.ResetAction();
            _rowToRotate = 0;
        }

        public override void SelectAction(int actionIndex)
        {
        }
    }
}
