using RPG.Combat.Actions;
using RPG.Combat.Grid;
using UnityEngine;

namespace RPG.Combat
{
    public class ApresentadorController : EntityController
    {
        [SerializeField] protected EntityScriptable _info;
        private float _currentMotivation;

        private int _maxRowRotations = 3;
        private int _rowToRotate = 0;

        private bool _hasActed = false;

        #region Properties

        public EntityScriptable Info { get { return _info; } }
        public float CurrentMotivation { get { return _currentMotivation; } }
        public int RowToRotate { get { return _rowToRotate; } }
        public int MaxRowRotations { get { return _maxRowRotations; } }
        public bool HasActed { get { return _hasActed; } }

        #endregion

        protected new void Start()
        {
            base.Start();
            _currentMotivation = CombatConstants.MAX_MOTIVATION_APRESENTADOR;
            MapManager.Instance.AddTileObject(_tileObject, Map.CENTER_POS);
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnApresentadorActionCompleted += ActionCompleted;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorActionCompleted -= ActionCompleted;
        }

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }

        protected override void OnSelected()
        {
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
            ActionsManager.Instance.OnApresentadorDamageTaken?.Invoke();

            Debug.Log(Info.name + " took " + damage + " damage\n Current Motivation Bar: " + _currentMotivation);
        }

        public void Rotate(int amount)
        {
            MapManager.Map.RotateRow(_rowToRotate, amount);
        }

        public void Rotate(int row, int amount)
        {
            MapManager.Map.RotateRow(row, amount);
        }

        public void ChangeRow(int amount)
        {
            _rowToRotate += amount;

            _rowToRotate += (Map.Rows - 1);
            _rowToRotate %= (Map.Rows - 1);
        }

        private void ActionCompleted()
        {
            _hasActed = true;
        }

        public void ResetAction()
        {
            _rowToRotate = 0;
            _hasActed = false;
        }
    }
}
