using RPG.Combat.Actions;
using RPG.Combat.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(TileObject))]
    public abstract class EntityController : MonoBehaviour
    {
        protected TileObject _tileObject;

        #region Properties
        public Vector2Int Position { get { return _tileObject.Position; } }
        public Direction Direction { get { return _tileObject.Direction; } }

        #endregion

        protected void Awake()
        {
            _tileObject = GetComponent<TileObject>();
        }

        public abstract void TakeDamage(float damage);

        
    }
}
