using UnityEngine;

using static LucasRozado.Utility.Unity;

namespace RPG.Input
{
    public class CursorTarget : InputComponent<CursorTarget.CursorTargetActionsHandler>
    {
        static public LayerMask CollisionLayers => CursorInput.CollisionLayers;

        [SerializeField] private GameObject collisionTarget;

        private void Start()
        {
            if (collisionTarget == null)
            {
                if (
                    Layer.IsLayerInMask(gameObject.layer, CollisionLayers)
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

                if (!Layer.IsLayerInMask(collisionTarget.layer, CollisionLayers))
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
                    from: CursorInput.Actions.HoverTarget,
                    derive: (target) =>
                    {
                        do
                        {
                            if (target == self.collisionTarget)
                            { return true; }
                            else if (target != null && target.transform.parent != null)
                            { target = target.transform.parent.gameObject; }
                        }
                        while (target != null && Layer.IsLayerInMask(target.layer, CollisionLayers));

                        return false;
                    }
                );

                bool DeriveClick(bool isClicked)
                {
                    bool isHovered = Hover.IsPressed;
                    return isHovered && isClicked;
                }

                LeftClick = DeriveHandler(
                    from: CursorInput.Actions.LeftClick,
                    derive: DeriveClick
                );
                MiddleClick = DeriveHandler(
                    from: CursorInput.Actions.MiddleClick,
                    derive: DeriveClick
                );
                RightClick = DeriveHandler(
                    from: CursorInput.Actions.RightClick,
                    derive: DeriveClick
                );
            }

            #endregion
        }
    }
}
