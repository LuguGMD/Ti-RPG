using UnityEngine;

using LucasRozado.Utility;

namespace RPG
{
    public class CursorTarget : InputComponent<CursorTarget.CursorTargetActionsHandler>
    {
        static public LayerMask CollisionLayer => CursorInput.Instance.CollisionLayer;

        [SerializeField] private new Collider collider;

        protected new void Awake()
        {
            base.Awake();

            if (collider == null)
            {
                if (TryGetComponent(out collider))
                {
                    Debug.LogWarning(
                        $"{gameObject.name}: " +
                        $"Um Collider para ser detectado pelo Cursor não foi definido." +
                        $"O componente {collider.GetType().Name} será utilizado."
                    );
                }
                else
                {
                    Debug.LogError(
                        $"{gameObject.name}: " +
                        $"Um Collider para ser detectado pelo Cursor não foi definido."
                    );
                }
            }

            if (!Layer.IsInMask(collider.gameObject.layer, CollisionLayer))
            {
                Debug.LogError(
                    $"{gameObject.name} > {collider.gameObject.name}: " +
                    "A Layer do colisor não é detectada pelo Cursor."
                );
            }

        }

        protected override CursorTargetActionsHandler SetupHandler() => new();
        public class CursorTargetActionsHandler : ActionsHandler
        {
            #region Derived Handlers

            public DerivedHandler<CursorTarget, bool> Hover;

            public DerivedHandler<bool, bool> LeftClick;
            public DerivedHandler<bool, bool> MiddleClick;
            public DerivedHandler<bool, bool> RightClick;

            public CursorTargetActionsHandler()
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
