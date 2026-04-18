using UnityEngine;

using LucasRozado.Utility;

namespace RPG
{
    public class CursorTarget : InputComponent<CursorTarget.CursorTargetActionsHandler>
    {
        static public LayerMask CollisionLayer => CursorInput.Instance.CollisionLayer;

        [SerializeField] private GameObject collisionTarget;

        private void Start()
        {
            if (collisionTarget == null)
            {
                if (
                    Layer.IsInMask(gameObject.layer, CollisionLayer)
                    && TryGetComponent<Collider>(out _)
                )
                {
                    Debug.LogWarning(
                        $"{gameObject.name}: " +
                        $"Um Target Collider para ser detectado pelo Cursor não foi definido.\n" +
                        $"O Collider do próprio objeto será utilizado."
                    );
                    collisionTarget = gameObject;
                }
                else
                {
                    Debug.LogWarning(
                        $"{gameObject.name}: " +
                        $"Um Collision Target para ser detectado pelo Cursor não foi definido."
                    );
                }
            }
            else
            {
                if (!collisionTarget.TryGetComponent<Collider>(out _))
                {
                    Debug.LogWarning(
                        $"{gameObject.name} > {collisionTarget.name}: " +
                        "O Collision Target não possui nenhum Collider."
                    );
                }

                if (!Layer.IsInMask(collisionTarget.layer, CollisionLayer))
                {
                    Debug.LogWarning(
                        $"{gameObject.name} > {collisionTarget.name}: " +
                        "A Layer do Collision Target não é detectada pelo Cursor."
                    );
                }
            }
        }

        protected override CursorTargetActionsHandler SetupHandler() => new(this);
        public class CursorTargetActionsHandler : ActionsHandler
        {
            #region Derived Handlers

            public DerivedHandler<GameObject, bool> Hover;

            public DerivedHandler<bool, bool> LeftClick;
            public DerivedHandler<bool, bool> MiddleClick;
            public DerivedHandler<bool, bool> RightClick;

            public CursorTargetActionsHandler(CursorTarget cursorTarget)
            {
                Hover = DeriveHandler(
                    from: CursorInput.Instance.Actions.HoverTarget,
                    derive: (target) =>
                    {
                        bool isTarget = target == cursorTarget.collisionTarget;
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
