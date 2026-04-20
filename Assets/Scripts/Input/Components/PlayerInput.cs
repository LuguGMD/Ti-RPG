using UnityEngine;
using static InputSystem_Actions;
using static UnityEngine.InputSystem.InputAction;

namespace RPG.Input
{
    // Essa classe é provisória e foi criada para testes e prova de conceito.
    // As PlayerActions provavelmente vão ser modificadas ou removidas futuramente.
    public class PlayerInput : InputComponent<PlayerInput.PlayerInputActionsHandler>
    {
        protected override PlayerInputActionsHandler GetHandler() => new();

        public class PlayerInputActionsHandler : ActionsHandler<PlayerActions>, IPlayerActions
        {
            public override PlayerActions InputActions => InputManager.Actions.Player;

            #region Input Actions

            public ButtonHandler Attack = new();
            void IPlayerActions.OnAttack(CallbackContext context) => Attack.Handle(context);

            public ButtonHandler Crouch = new();
            void IPlayerActions.OnCrouch(CallbackContext context) => Crouch.Handle(context);

            public ButtonHandler Interact = new();
            void IPlayerActions.OnInteract(CallbackContext context) => Interact.Handle(context);

            public ButtonHandler Jump = new();
            void IPlayerActions.OnJump(CallbackContext context) => Jump.Handle(context);

            public ValueHandler<Vector2> Look = new();
            void IPlayerActions.OnLook(CallbackContext context) => Look.Handle(context);

            public ValueHandler<Vector2> Move = new();
            void IPlayerActions.OnMove(CallbackContext context) => Move.Handle(context);

            public ButtonHandler Next = new();
            void IPlayerActions.OnNext(CallbackContext context) => Next.Handle(context);

            public ButtonHandler Previous = new();
            void IPlayerActions.OnPrevious(CallbackContext context) => Previous.Handle(context);

            public ButtonHandler Sprint = new();
            void IPlayerActions.OnSprint(CallbackContext context) => Sprint.Handle(context);

            #endregion
        }
    }
}
