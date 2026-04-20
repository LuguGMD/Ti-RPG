using System;

using static UnityEngine.InputSystem.InputAction;

namespace RPG.Input
{
    public abstract class InputHandler { }
    public abstract class InputHandler<TValue> : InputHandler
    {
        private Action onStart;
        private Action<TValue> onUpdate;
        private Action onCancel;

        public TValue LastValue { get; private set; }
        public bool IsPressed { get; private set; }

        public abstract TValue GetValue(CallbackContext context);

        public virtual void Handle(CallbackContext context)
        {
            TValue value = GetValue(context);

            bool actionStarted = context.started;
            bool actionUpdated = context.performed || context.canceled;
            bool actionCancelled = context.canceled;

            if (actionStarted)
            { EmitOnStart(value); }

            if (actionUpdated)
            { EmitOnUpdate(value); }

            if (actionCancelled)
            { EmitOnCancel(value); }
        }

        #region Signal Emitters

        protected virtual void EmitOnStart(TValue value)
        {
            IsPressed = true;
            onStart?.Invoke();
        }

        protected virtual void EmitOnUpdate(TValue value)
        {
            onUpdate?.Invoke(value);
            LastValue = value;
        }

        protected virtual void EmitOnCancel(TValue value)
        {
            IsPressed = false;
            onCancel?.Invoke();
        }

        #endregion

        #region Handle Signals

        public void Handle(
            Action onStart = null,
            Action<TValue> onUpdate = null,
            Action onCancel = null
        )
        {
            if (onStart != null) OnStart(onStart);
            if (onUpdate != null) OnUpdate(onUpdate);
            if (onCancel != null) OnCancel(onCancel);
        }

        public void OnStart(Action execute) => onStart += execute;

        public void OnUpdate(Action<TValue> execute) => onUpdate += execute;

        public void OnCancel(Action execute) => onCancel += execute;

        #endregion

        #region Signal Remover

        public SignalRemover Remove => new(this);
        public readonly struct SignalRemover
        {
            private readonly InputHandler<TValue> self;
            public SignalRemover(InputHandler<TValue> self)
            { this.self = self; }

            public readonly void OnStart(Action callback) => self.onStart -= callback;

            public readonly void OnUpdate(Action<TValue> callback) => self.onUpdate -= callback;

            public readonly void OnCancel(Action callback) => self.onCancel -= callback;
        }

        #endregion
    }
}
