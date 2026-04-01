using System;

using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;
using static UnityEngine.InputSystem.InputAction;

namespace RPG
{
    public class PlayerInputManager : InputManager<PlayerActions, IPlayerActions>
    {
        protected override InputActionMap ActionMap => InputManager.Actions.Player.Get();
        public  PlayerInputHandler Actions = new();
        protected override InputHandler<PlayerActions, IPlayerActions> Handler => Actions;

        public class PlayerInputHandler : InputHandler<PlayerActions, IPlayerActions>, IPlayerActions
        // Se houver erro de interface não implementada, é por que houveram mudanças nas InputActions
        // Para corrigir, essa é a única classe que precisa ser modificada, copiando o padrão dos outros inputs:
        {
            protected override Action<IPlayerActions> SetCallbacks
            => InputManager.Actions.Player.SetCallbacks;
            protected override Action<IPlayerActions> RemoveCallbacks
            => InputManager.Actions.Player.RemoveCallbacks;

            /* Action do tipo Button:
            public ButtonAction [ACTION] = new();
            void IPlayerActions.On[ACTION](CallbackContext context) => [ACTION].Handle(context);
             
             * Action do tipo Value ou Passthrough:
            public ValueAction<[TIPO]> [ACTION] = new();
            void IPlayerActions.On[ACTION](CallbackContext context) => [ACTION].Handle(context);
             */

            public ButtonAction Attack = new();
            void IPlayerActions.OnAttack(CallbackContext context) => Attack.Handle(context);

            public ButtonAction Crouch = new();
            void IPlayerActions.OnCrouch(CallbackContext context) => Crouch.Handle(context);

            public ButtonAction Interact = new();
            void IPlayerActions.OnInteract(CallbackContext context) => Interact.Handle(context);

            public ButtonAction Jump = new();
            void IPlayerActions.OnJump(CallbackContext context) => Jump.Handle(context);

            public ValueAction<Vector2> Look = new();
            void IPlayerActions.OnLook(CallbackContext context) => Look.Handle(context);

            public ValueAction<Vector2> Move = new();
            void IPlayerActions.OnMove(CallbackContext context) => Move.Handle(context);

            public ButtonAction Next = new();
            void IPlayerActions.OnNext(CallbackContext context) => Next.Handle(context);

            public ButtonAction Previous = new();
            void IPlayerActions.OnPrevious(CallbackContext context) => Previous.Handle(context);

            public ButtonAction Sprint = new();
            void IPlayerActions.OnSprint(CallbackContext context) => Sprint.Handle(context);
        }

    }
}
