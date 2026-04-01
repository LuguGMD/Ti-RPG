using System;

using static UnityEngine.InputSystem.InputAction;

namespace RPG
{
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
