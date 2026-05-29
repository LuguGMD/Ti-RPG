using UnityEngine;
using UnityEngine.EventSystems;
using static InputSystem_Actions;
using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG.Input
{
    public class CursorInput : InputComponent<CursorInput.CursorInputActionsHandler>
    {
        static public new CursorInputActionsHandler Actions => Singleton<CursorInputActionsHandler>.Instance;
        public static CursorInput Instance;

        [SerializeField] private LayerMask ignoreCollisionLayers;
        static public LayerMask CollisionLayers => Utility.Get(Instance.ignoreCollisionLayers).Inverse;


        protected new void Awake()
        {
            Singleton();

            if (Instance == this)
            { base.Awake(); }
        }

        protected new void OnEnable()
        {
            Singleton();

            if (Instance == this)
                base.OnEnable();
        }

        protected new void OnDisable()
        {
            if (Instance == this)
                base.OnDisable();
        }

        private void Singleton()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else if(Instance != this)
            {
                Destroy(gameObject);
            }
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
                    return UnityEngine.Camera.main.ScreenPointToRay(position);
                });

                HoverTarget = DeriveHandler(from: Ray, derive: (ray) =>
                {
                    Vector2 cursorPosition = Position.LastValue;
                    var eventSystemUtility = Utility.Get(EventSystem.current);

                    if (eventSystemUtility.IsCursorOverUIElement(cursorPosition))
                    { return null; }

                    if (Physics.Raycast(ray,
                        maxDistance: Mathf.Infinity,
                        layerMask: CollisionLayers,
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
