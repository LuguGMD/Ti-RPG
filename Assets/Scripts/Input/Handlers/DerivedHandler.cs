using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG.Input
{
    public class DerivedHandler<TInput, TValue> : InputHandler<TValue>, ISimpleProcess
    {
        private readonly InputHandler<TInput> baseHandler;

        public delegate TValue Deriver(TInput input);
        private readonly Deriver deriver;

        public DerivedHandler(InputHandler<TInput> from, Deriver derive)
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
}
