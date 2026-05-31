using FMODUnity;
using RPG.Combat.Actions;
using RPG.Combat.Grid;
using System;
using UnityEngine;

namespace RPG.Combat
{
    public class ApresentadorController : EntityController
    {
        [SerializeField] protected EntityScriptable _info;
        private float _currentMotivation;

        private int _maxRowRotations = 3;
        private int _rowToRotate = 0;

        

        #region Properties

        public EntityScriptable Info { get { return _info; } }
        public float CurrentMotivation { get { return _currentMotivation; } }
        public int RowToRotate { get { return _rowToRotate; } }
        public int MaxRowRotations { get { return _maxRowRotations; } }

        #endregion

        protected new void Start()
        {
            base.Start();
            Initialize();
        }

        private void Initialize()
        {
            _currentMotivation = CombatConstants.MAX_MOTIVATION_APRESENTADOR;
            MapManager.Instance.AddTileObject(_tileObject, Map.CENTER_POS);
            AdjsutGameSpeed();
        }

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected override void OnSelected()
        {
            if (!CombatManager.HasCombatStarted) return;
            if (!CombatManager.IsActionInProgress) return;

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

            ActionsManager.Instance.OnApresentadorDamageTaken?.Invoke();
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

        public void CompleteAction()
        {
            _hasActed = true;
        }

        public override void ResetAction()
        {
            base.ResetAction();
            _rowToRotate = 0;
        }
    }
}
