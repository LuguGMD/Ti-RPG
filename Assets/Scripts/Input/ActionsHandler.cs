using System.Collections.Generic;

using static LucasRozado.Utility.Object;
using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG
{
    public abstract class ActionsHandler
    {
        #region Derived Handlers Logic

        private readonly HashSet<ISimpleProcess> derivedHandlers = new();

        protected DerivedHandler<TInput, TValue> DeriveHandler<TInput, TValue>
        (
            InputHandler<TInput> from,
            DerivedHandler<TInput, TValue>.Deriver derive
        )
        {
            DerivedHandler<TInput, TValue> derivedHandler = new(from, derive);
            derivedHandlers.Add(derivedHandler);
            return derivedHandler;
        }

        private void StartDerivedHandlers()
        {
            foreach (ISimpleProcess handler in derivedHandlers)
            { handler.Start(); }
        }

        private void StopDerivedHandlers()
        {
            foreach (ISimpleProcess handler in derivedHandlers)
            { handler.Stop(); }
        }

        #endregion

        public void Start()
        {
            StartDerivedHandlers();
            OnStart();
        }

        public void Stop()
        {
            StopDerivedHandlers();
            OnStop();
        }

        public virtual void OnStart() {}
        public virtual void OnStop() {}
    }

    public abstract class ActionsHandler<TActions> : ActionsHandler
        where TActions : struct
    {
        public abstract TActions InputActions { get; }
        public ActionsHandler()
        {
            InputManager.Register(this);
        }

        protected void Handle<TValue>(CallbackContext context, InputHandler<TValue> handler)
        {
            handler.Handle(context);
        }

        public override void OnStart()
        { GetUtils(InputActions).Invoke("AddCallbacks", this); }
        public override void OnStop()
        { GetUtils(InputActions).Invoke("RemoveCallbacks", this); }

        ~ActionsHandler()
        {
            InputManager.Unregister(this);
        }
    }
}
