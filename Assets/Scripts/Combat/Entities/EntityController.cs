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

        #region Properties
        public Vector2Int Position { get { return _tileObject.Position; } }
        public DirectionEnum Direction { get { return _tileObject.Direction; } }

        #endregion

        protected void Awake()
        {
            _tileObject = GetComponent<TileObject>();
            _cursorTarget = GetComponent<CursorTarget>();
        }

        protected void Start()
        {
            _cursorTarget.Actions.LeftClick.OnStart(OnSelected);
            _cursorTarget.Actions.Hover.OnStart(OnHover);
        }

        protected virtual void OnSelected()
        {
            ActionsManager.Instance.OnEntitySelected?.Invoke(this);
        }

        protected virtual void OnHover()
        {
            ActionsManager.Instance.OnEntityHovered?.Invoke(this);
        }

        public abstract void TakeDamage(float damage);

        public abstract EntityScriptable GetEntityInfo();


        
    }
}
