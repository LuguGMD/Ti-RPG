using System;

using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG
{
    public abstract class ActionHandler { }
    public abstract class ActionHandler<TValue> : ActionHandler
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
            private readonly ActionHandler<TValue> self;
            public SignalRemover(ActionHandler<TValue> self)
            { this.self = self; }

            public readonly void OnStart(Action callback) => self.onStart -= callback;

            public readonly void OnUpdate(Action<TValue> callback) => self.onUpdate -= callback;

            public readonly void OnCancel(Action callback) => self.onCancel -= callback;
        }

        #endregion
    }

    public class ValueHandler<TValue> : ActionHandler<TValue>
        where TValue : struct
    {
        public override TValue GetValue(CallbackContext context)
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

        public override TValue GetValue(CallbackContext context)
        {
            TInput input = baseHandler.GetValue(context);
            TValue value = deriver(input);
            return value;
        }

        public override void Handle(CallbackContext context)
        {
            baseHandler.Handle(context);
        }

        private void Derive(TInput input)
        {
            TValue value = deriver(input);

            bool isSameValue = Equals(value, LastValue);
            bool isDefaultValue = Equals(value, default(TValue));

            bool actionStarted = !IsPressed && !isDefaultValue;
            bool actionUpdated = !isSameValue;
            bool actionCancelled = !isSameValue && isDefaultValue;
            
            if (actionStarted)
            { EmitOnStart(value); }

            if (actionUpdated)
            { EmitOnUpdate(value); }

            if (actionCancelled)
            { EmitOnCancel(value); }
        }

        public void Start()
        {
            baseHandler.OnUpdate(Derive);
        }

        public void Stop()
        {
            baseHandler.Remove.OnUpdate(Derive);
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
