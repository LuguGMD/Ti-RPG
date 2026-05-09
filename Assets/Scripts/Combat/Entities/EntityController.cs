using RPG.Combat.Actions;
using RPG.Combat.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Input;

namespace RPG.Combat
{
    [RequireComponent(typeof(TileObject), typeof(CursorTarget))]
    public abstract class EntityController : MonoBehaviour
    {
        protected TileObject _tileObject;
        protected CursorTarget _cursorTarget;
        protected Animator[] _animators;

        #region Properties
        public Vector2Int Position { get { return _tileObject.Position; } }
        public DirectionEnum Direction { get { return _tileObject.Direction; } }
        public TileObject TileObject { get { return _tileObject; } }

        #endregion

        protected void Awake()
        {
            _tileObject = GetComponent<TileObject>();
            _cursorTarget = GetComponent<CursorTarget>();
            _animators = GetComponentsInChildren<Animator>();
        }

        protected void Start()
        {
            _cursorTarget.Actions.LeftClick.OnStart(OnSelected);
            _cursorTarget.Actions.Hover.OnStart(OnHover);
        }

        protected void OnEnable()
        {
            ActionsManager.Instance.OnCombatSpeedChanged += AdjsutGameSpeed;
            ActionsManager.Instance.OnPreviewTileSelected += CheckSelected;
        }

        protected void OnDisable()
        {
            ActionsManager.Instance.OnCombatSpeedChanged -= AdjsutGameSpeed;
            ActionsManager.Instance.OnPreviewTileSelected -= CheckSelected;
        }

        protected void AdjsutGameSpeed()
        {
            foreach (Animator animator in _animators)
            {
                animator.SetFloat("GameSpeed", CombatManager.CombatSpeed);
            }
        }

        private void CheckSelected(Vector2Int selectedPositon)
        {
            if(selectedPositon == Position)
            {
                OnSelected();
            }
        }

        protected virtual void OnSelected()
        {
            ActionsManager.Instance.OnEntitySelected?.Invoke(this);
        }

        protected virtual void OnHover()
        {
            ActionsManager.Instance.OnEntityHovered?.Invoke(this);
        }

        protected void SetAnimationTrigger(string trigger)
        {
            foreach (Animator animator in _animators)
            {
                animator.SetTrigger(trigger);
            }
        }
        protected void SetAnimationInt(string intName, int intValue)
        {
            foreach(Animator animator in _animators)
            {
                animator.SetInteger(intName, intValue);
            }
        }
        protected void SetAnimationBool(string boolName, bool boolValue)
        {
            foreach (Animator animator in _animators)
            {
                animator.SetBool(boolName, boolValue);
            }
        }

        public virtual void TakeDamage(float damage)
        {
            SetAnimationTrigger("TookDamage");
            CheckDefeated();
        }

        public abstract void Heal(float heal);
        protected abstract void CheckDefeated();
        protected virtual void Defeated()
        {
            SetAnimationTrigger("Died");
        }

        public abstract EntityScriptable GetEntityInfo();



    }
}
