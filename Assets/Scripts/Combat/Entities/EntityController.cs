using RPG.Combat.Actions;
using RPG.Combat.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Input;
using FMODUnity;
using RPG.Audio;

namespace RPG.Combat
{
    [RequireComponent(typeof(TileObject), typeof(CursorTarget))]
    public abstract class EntityController : MonoBehaviour
    {
        [SerializeField] protected StageEntityScriptable _info;
        protected TileObject _tileObject;
        protected CursorTarget _cursorTarget;
        protected Animator[] _animators;
        protected bool _hasActed = false;
        protected bool _isHovered = false;

        [SerializeField] private EventReference _damagedSFX;

        #region Properties
        public Vector2Int Position { get { return _tileObject.Position; } }
        public StageEntityScriptable Info { get { return _info; } }
        public DirectionEnum Direction { get { return _tileObject.Direction; } }
        public TileObject TileObject { get { return _tileObject; } }
        public bool HasActed { get { return _hasActed; } }

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
            _cursorTarget.Actions.Hover.OnStart(OnHoverStart);
            _cursorTarget.Actions.Hover.OnCancel(OnHoverEnd);
        }

        protected void OnDestroy()
        {
            _cursorTarget.Actions.LeftClick.Stop();
            _cursorTarget.Actions.Hover.Stop();
        }

        protected void OnEnable()
        {
            ActionsManager.Instance.OnCombatSpeedChanged += AdjsutGameSpeed;
            ActionsManager.Instance.OnPreviewTileSelected += CheckSelected;
            ActionsManager.Instance.OnEnemyTurnEnded += ResetAction;
            ActionsManager.Instance.OnTileHovered += CheckHovered;
        }

        protected void OnDisable()
        {
            ActionsManager.Instance.OnCombatSpeedChanged -= AdjsutGameSpeed;
            ActionsManager.Instance.OnPreviewTileSelected -= CheckSelected;
            ActionsManager.Instance.OnEnemyTurnEnded -= ResetAction;
            ActionsManager.Instance.OnTileHovered -= CheckHovered;
        }

        public virtual void ResetAction()
        {
            _hasActed = false;
        }

        protected void AdjsutGameSpeed()
        {
            foreach (Animator animator in _animators)
            {
                animator.SetFloat("GameSpeed", CombatManager.CombatSpeed);
            }
        }

        protected virtual void CheckHovered(Vector2Int hoveredPositon)
        {
            if(!_isHovered && hoveredPositon == Position)
            {
                _isHovered = true;
            }
            else if(_isHovered && hoveredPositon != Position)
            {
                _isHovered = false;
            }
        }

        protected void CheckSelected(Vector2Int selectedPositon)
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

        protected virtual void OnHoverStart()
        {
            ActionsManager.Instance.OnEntityHovered?.Invoke(this);
        }

        protected virtual void OnHoverEnd()
        {
            
        }

        public void SetAnimationTrigger(string trigger)
        {
            foreach (Animator animator in _animators)
            {
                animator.SetTrigger(trigger);
            }
        }
        public void SetAnimationInt(string intName, int intValue)
        {
            foreach(Animator animator in _animators)
            {
                animator.SetInteger(intName, intValue);
            }
        }
        public void SetAnimationBool(string boolName, bool boolValue)
        {
            foreach (Animator animator in _animators)
            {
                animator.SetBool(boolName, boolValue);
            }
        }

        protected bool IsInActionAnimation()
        {
            foreach (Animator animator in _animators)
            {
                if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Action") || animator.IsInTransition(0))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void TakeDamage(float damage)
        {
            AudioManager.Instance.PlayOneShot(_damagedSFX);
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
