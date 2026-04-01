using System;
using UnityEngine;

namespace RPG
{
    public abstract class InputHandler<TActions, THandler>
        where TActions : struct
        where THandler : class
    {
        protected abstract Action<THandler> SetCallbacks { get; }
        protected abstract Action<THandler> RemoveCallbacks { get; }

        public void Start()
        { SetCallbacks(this as THandler); }
        public void Stop()
        { RemoveCallbacks(this as THandler); }
    }
}
