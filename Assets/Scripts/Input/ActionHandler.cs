using System;

using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG
{
    public abstract class ActionHandler { }
    public abstract class ActionHandler<TValue> : ActionHandler
    {
        private Action<CallbackContext> onHandle;

        private Action onStart;
        private Action<TValue> onUpdate;
        private Action onCancel;

        public TValue LastValue { get; private set; }
        public bool IsPressed { get; private set; }


        protected abstract TValue GetValue(CallbackContext context);

        public void Handle(CallbackContext context)
        {
            TValue value = GetValue(context);
            Handle(context, value);
        }

        protected virtual void Handle(CallbackContext context, TValue value)
        {
            bool actionStarted = context.started;
            if (actionStarted)
            { EmitOnStart(value); }

            bool actionUpdated = context.performed || context.canceled;
            if (actionUpdated)
            { EmitOnUpdate(value); }

            bool actionCancelled = context.canceled;
            if (actionCancelled)
            { EmitOnCancel(value); }

            EmitOnHandle(context);
        }

        #region Signal Emitters

        protected virtual void EmitOnHandle(CallbackContext context)
        {
            onHandle?.Invoke(context);
        }

        protected virtual void EmitOnStart(TValue value)
        {
            onStart?.Invoke();
            IsPressed = true;
        }

        protected virtual void EmitOnUpdate(TValue value)
        {
            onUpdate?.Invoke(value);
            LastValue = value;
        }

        protected virtual void EmitOnCancel(TValue value)
        {
            onCancel?.Invoke();
            IsPressed = false;
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

        public void OnHandle(Action<CallbackContext> execute) => onHandle += execute;

        #endregion

        #region Signal Remover

        public SignalRemover Remove => new(this);
        public readonly struct SignalRemover
        {
            private readonly ActionHandler<TValue> self;
            public SignalRemover(ActionHandler<TValue> self)
            { this.self = self; }

            public readonly void OnStart(Action callback) => self.onStart -= callback;

            public readonly void OnUpdate(Action<TValue> callback) => self.onUpdate -= callback;

            public readonly void OnCancel(Action callback) => self.onCancel -= callback;

            public readonly void OnHandle(Action<CallbackContext> callback) => self.onHandle -= callback;
        }

        #endregion
    }

    public class ValueHandler<TValue> : ActionHandler<TValue>
        where TValue : struct
    {
        protected override TValue GetValue(CallbackContext context)
        {
            TValue value = context.ReadValue<TValue>();
            return value;
        }
    }

    public class DerivedHandler<TInput, TValue> : ActionHandler<TValue>, ISimpleProcess
    {
        private readonly ActionHandler<TInput> baseHandler;

        public delegate TValue Deriver(TInput input);
        private readonly Deriver deriver;

        public DerivedHandler(ActionHandler<TInput> from, Deriver derive)
        {
            baseHandler = from;
            deriver = derive;

            Start();
        }

        protected override TValue GetValue(CallbackContext context)
        {
            TInput input = baseHandler.LastValue;
            TValue value = deriver(input);
            return value;
        }

        protected override void Handle(CallbackContext context, TValue value)
        {
            bool isSameValue = Equals(value, LastValue);
            bool isDefaultValue = Equals(value, default);

            bool actionStarted = !IsPressed && !isDefaultValue;
            if (actionStarted)
            { EmitOnStart(value); }

            bool actionUpdated = !isSameValue;
            if (actionUpdated)
            { EmitOnUpdate(value); }

            bool actionCancelled = !isSameValue && isDefaultValue;
            if (actionCancelled)
            { EmitOnCancel(value); }

            if (actionUpdated)
            { EmitOnHandle(context); }
        }

        public void Start()
        {
            baseHandler.OnHandle(Handle);
        }

        public void Stop()
        {
            baseHandler.Remove.OnHandle(Handle);
        }
    }

    public class ButtonHandler : DerivedHandler<float, bool>
    {
        public ButtonHandler() : base(
            from: new ValueHandler<float>(),
            derive: (value) => value != default
        )
        { }
    }
}
