using UnityEngine;

using LucasRozado.Utility;

namespace RPG
{
    public class CursorTarget : InputComponent<CursorTarget.CursorTargetActionsHandler>
    {
        static public LayerMask CollisionLayer => CursorInput.TargetCollisionLayer;

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
                if (
                    !collisionTarget.TryGetComponent<Collider>(out _)
                    && collisionTarget.GetComponentInChildren<Collider>() == null
                )
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

        protected override CursorTargetActionsHandler GetHandler() => new(this);
        public class CursorTargetActionsHandler : ActionsHandler
        {
            private readonly CursorTarget self;
            public CursorTargetActionsHandler(CursorTarget self)
            { this.self = self; }

            #region Derived Handlers

            public DerivedHandler<GameObject, bool> Hover;

            public DerivedHandler<bool, bool> LeftClick;
            public DerivedHandler<bool, bool> MiddleClick;
            public DerivedHandler<bool, bool> RightClick;

            protected override void DeriveHandlers()
            {
                Hover = DeriveHandler(
                    from: CursorInput.Instance.Actions.HoverTarget,
                    derive: (target) =>
                    {
                        do
                        {
                            if (target == self.collisionTarget)
                            { return true; }
                            else
                            { target = target?.transform.parent?.gameObject; }
                        }
                        while (target != null && Layer.IsInMask(target.layer, CollisionLayer));
                        
                        return false;
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
