using UnityEngine;

using LucasRozado.Utility;
using System.Collections.Generic;

namespace RPG
{
    public class CursorTarget : InputComponent<CursorTarget.Handler>
    {
        static public LayerMask CollisionLayer => CursorInput.Instance.CollisionLayer;

        protected override Handler SetupHandler() => new();

        [SerializeField] private new Collider collider;
        protected new void Awake()
        {
            base.Awake();
         
            if (collider == null)
            {
                Debug.LogError(
                $"{gameObject.name}: O Collider detectado pelo Cursor não foi definido."
                );
            }
            else if (!Layer.IsInMask(collider.gameObject.layer, CollisionLayer))
            {
                Debug.LogError(
                $"{gameObject.name} > {collider.gameObject.name}: A Layer do colisor não é detectada pelo Cursor."
                );
            }

        }

        public class Handler : ActionsHandler
        {
            #region Derived Handlers
            
            public DerivedHandler<CursorTarget, bool> Hover;

            public DerivedHandler<bool, bool> LeftClick;
            public DerivedHandler<bool, bool> MiddleClick;
            public DerivedHandler<bool, bool> RightClick;

            public Handler()
            {
                Hover = DeriveHandler(
                    from: CursorInput.Instance.Actions.HoverTarget,
                    derive: (target) =>
                    {
                        bool hasTarget = target != null;
                        bool isTarget = hasTarget && target.Actions == this;
                        return isTarget;
                    }
                );

                bool DeriveClick(bool isClicked)
                {
                    bool isHovered = Hover.IsPressed;
                    return isHovered && isClicked;
                }
                
                LeftClick = DeriveHandler(
                    from: CursorInput.Instance.Actions.LeftClick,
                    derive: DeriveClick
                );
                MiddleClick = DeriveHandler(
                    from: CursorInput.Instance.Actions.MiddleClick,
                    derive: DeriveClick
                );
                RightClick = DeriveHandler(
                    from: CursorInput.Instance.Actions.RightClick,
                    derive: DeriveClick
                );
            }

            #endregion
        }
    }
}
