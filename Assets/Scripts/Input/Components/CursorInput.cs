using UnityEngine;
using static InputSystem_Actions;
using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG.Input
{
    public class CursorInput : InputComponent<CursorInput.CursorInputActionsHandler>
    {
        static public new CursorInputActionsHandler Actions => Singleton<CursorInputActionsHandler>.Instance;
        private static CursorInput _instance;
        
        [SerializeField] private LayerMask targetCollisionLayer;
        static public LayerMask TargetCollisionLayer => Instance.targetCollisionLayer;

        public static CursorInput Instance {  get { return _instance; } }

        protected new void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            base.Awake();
            //Singleton<CursorInput>.Create(this);
        }

        protected override CursorInputActionsHandler GetHandler() => Singleton<CursorInputActionsHandler>.Instance;
        public class CursorInputActionsHandler : ActionsHandler<CursorActions>, ICursorActions
        {

            public override CursorActions InputActions => InputManager.Actions.Cursor;

            #region Input Actions

            public readonly ValueHandler<Vector2> Position = new();
            void ICursorActions.OnPosition(CallbackContext context) => Position.Handle(context);

            public readonly ButtonHandler LeftClick = new();
            void ICursorActions.OnClick(CallbackContext context) => LeftClick.Handle(context);

            public readonly ButtonHandler MiddleClick = new();
            void ICursorActions.OnMiddleClick(CallbackContext context) => MiddleClick.Handle(context);

            public readonly ButtonHandler RightClick = new();
            void ICursorActions.OnRightClick(CallbackContext context) => RightClick.Handle(context);

            public readonly ValueHandler<Vector2> ScrollWheel = new();
            void ICursorActions.OnScrollWheel(CallbackContext context) => ScrollWheel.Handle(context);

            #endregion

            #region Derived Handlers

            public DerivedHandler<Vector2, Ray> Ray;
            public DerivedHandler<Ray, GameObject> HoverTarget;

            protected override void DeriveHandlers()
            {
                Ray = DeriveHandler(from: Position, derive: (position) =>
                {
                    return Camera.main.ScreenPointToRay(position);
                });

                HoverTarget = DeriveHandler(from: Ray, derive: (ray) =>
                {
                    if (Physics.Raycast(ray,
                        maxDistance: Mathf.Infinity,
                        layerMask: TargetCollisionLayer,
                        hitInfo: out RaycastHit hit
                    ))
                    { return hit.collider.gameObject; }
                    else
                    { return null; }
                });
            }

            #endregion
        }
    }
}
