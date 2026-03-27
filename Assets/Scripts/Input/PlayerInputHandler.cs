using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;
using static UnityEngine.InputSystem.InputAction;

namespace RPG
{
    public abstract class InputManager : MonoBehaviour
    {
        static readonly public InputSystem_Actions Actions = new();
        static readonly private Dictionary<InputActionMap, List<InputManager>> subManagers = new();

        abstract protected InputActionMap ActionMap { get; }

        protected void Awake()
        {
            if (!subManagers.ContainsKey(ActionMap))
            { subManagers[ActionMap] = new List<InputManager> { this }; }
            else
            { subManagers[ActionMap].Add(this); }
        }
    }

    [RequireComponent(typeof(PlayerInputManager))]
    public class ScriptDeExemplo : MonoBehaviour
    {
        private PlayerInputManager inputManager;
        private void Awake()
        {
            inputManager = GetComponent<PlayerInputManager>();
        }

        private void Update()
        {
            if (inputManager.Actions.Look.LastValue == Vector2.right)
            {
                Debug.Log("processando alguma coisa só quando olha pra direita");
            }
        }
    }

    public class PlayerInputManager : InputManager
    {
        protected override InputActionMap ActionMap => InputManager.Actions.Player.Get();

        public readonly new PlayerInputHandler Actions = new();

        public abstract class InputHandler
        {
            public void Enable()
            {


            }

            public void Disable()
            {

            }
        }

        public class PlayerInputHandler : InputHandler, IPlayerActions
        // Se houver erro de interface não implementada, é por que houveram mudanças nas InputActions
        // Para corrigir, essa é a única classe que precisa ser modificada, copiando o padrão dos outros inputs:
        {
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

        public abstract class InputAction
        {
            private Action onStart;
            private Action onCancel;

            // Handle Actions
            protected virtual void HandleOnStart(CallbackContext context)
            { onStart?.Invoke(); }
            protected virtual void HandleOnCancel(CallbackContext context)
            { onCancel?.Invoke(); }
            public virtual void Handle(CallbackContext context)
            {
                bool actionStarted = context.started;
                bool actionCancelled = context.canceled;

                if (actionStarted)
                { HandleOnStart(context); }

                if (actionCancelled)
                { HandleOnStart(context); }
            }

            // Handle Signals
            public void OnStart(Action execute) => onStart += execute;
            public void OnCancel(Action execute) => onCancel += execute;
            public void Handle(
                Action onStart = null,
                Action onCancel = null
            )
            {
                if (onStart != null) OnStart(onStart);
                if (onCancel != null) OnCancel(onCancel);
            }
        }
        public class ValueAction<TValue> : InputAction
            where TValue : struct
        {
            private Action<TValue> onUpdate;
            public TValue LastValue { get; private set; }

            // Handle Actions
            protected void HandleOnUpdate(CallbackContext context)
            {
                TValue input = context.ReadValue<TValue>();
                onUpdate?.Invoke(input);

                LastValue = input;
            }
            public override void Handle(CallbackContext context)
            {
                base.Handle(context);

                bool actionUpdated = context.performed || context.canceled;

                if (actionUpdated)
                { HandleOnUpdate(context); }
            }

            // Handle Signals
            public void OnUpdate(Action<TValue> execute) => onUpdate += execute;
            public void Handle(
                Action<TValue> onUpdate,
                Action onStart = null,
                Action onCancel = null
            )
            {
                Handle(onStart, onCancel);
                OnUpdate(onUpdate);
            }
        }
        public class ButtonAction : InputAction
        {
            public bool IsHeld { get; private set; }

            // Handle Actions
            protected override void HandleOnStart(CallbackContext context)
            {
                base.HandleOnStart(context);
                IsHeld = true;
            }
            protected override void HandleOnCancel(CallbackContext context)
            {
                base.HandleOnCancel(context);
                IsHeld = false;
            }
        }
    }
}
