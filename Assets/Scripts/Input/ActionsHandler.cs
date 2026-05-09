using System;
using System.Collections.Generic;

using static UnityEngine.InputSystem.InputAction;

using LucasRozado.Utility;

namespace RPG.Input
{
    public abstract class ActionsHandler
    {
        private HashSet<ISimpleProcess> derivedHandlers;

        public ActionsHandler() : base()
        {
            derivedHandlers = new();
            DeriveHandlers();
        }

        #region Derived Handlers Logic

        protected virtual void DeriveHandlers() { }

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

        public virtual void OnStart() { }
        public virtual void OnStop() { }
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
        { Utility.Get(InputActions).Invoke("AddCallbacks", this); }
        public override void OnStop()
        { Utility.Get(InputActions).Invoke("RemoveCallbacks", this); }

        ~ActionsHandler()
        {
            InputManager.Unregister(this);
        }
    }
}
