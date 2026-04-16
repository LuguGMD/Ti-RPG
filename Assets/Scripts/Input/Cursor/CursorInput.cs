using UnityEngine;
using static InputSystem_Actions;
using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG
{
    public class CursorInput : InputComponent<CursorInput.Handler>
    {
        #region Singleton Logic

        static private CursorInput instance;
        static public CursorInput Instance => instanceGetter.Invoke();
        static private Getter<CursorInput> instanceGetter = GetNoInstance;

        static private CursorInput GetNoInstance()
        {
            Debug.LogError(
                $"{nameof(CursorInput)}: " +
                "Tried getting a Singleton's instance, " +
                "but none were ever created."
            );
            return null;
        }
        
        static private void SetInstance(CursorInput instance)
        {
            instanceGetter = GetInstance;
            CursorInput.instance = instance;
        }
        
        static private CursorInput GetInstance() => instance;
        
        #endregion
        
        public LayerMask CollisionLayer;

        protected new void Awake()
        {
            base.Awake();

            if (instance == null)
            { SetInstance(this); }
            else
            {
                Debug.LogWarning(
                    $"{gameObject.name}: " +
                    "Another instance of a Singleton class was created."
                );
            }
        }

        protected override Handler SetupHandler() => Handler.Instance;
        public class Handler : ActionsHandler<CursorActions>, ICursorActions
        {

            public override CursorActions InputActions => InputManager.Actions.Cursor;

            #region Singleton Logic

            static private Handler instance;
            static public Handler Instance => instanceGetter.Invoke();
            static private Getter<Handler> instanceGetter = GetFirstInstance;
            
            static private Handler GetInstance() => instance;
            
            static private Handler GetFirstInstance()
            {
                instance = new();
                instanceGetter = GetInstance;
                return instance;
            }

            #endregion

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

            public readonly DerivedHandler<Vector2, Ray> Ray;
            public readonly DerivedHandler<Ray, CursorTarget> HoverTarget;

            public Handler()
            {
                Ray = new(from: Position, derive: (position) =>
                {
                    return Camera.main.ScreenPointToRay(position);
                });

                HoverTarget = new(from: Ray, derive: (ray) =>
                {
                    if (Physics.Raycast(ray,
                        maxDistance: Mathf.Infinity,
                        layerMask: CursorTarget.CollisionLayer,
                        hitInfo: out RaycastHit hit
                    ))
                    {
                        if (hit.collider.TryGetComponent(out CursorTarget target))
                        { return target; }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                });
            }

            #endregion
        }
    }
}
