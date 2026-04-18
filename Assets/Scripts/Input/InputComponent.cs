using System;

using UnityEngine;

namespace RPG
{
    public abstract class InputComponent<THandler> : MonoBehaviour
        where THandler : ActionsHandler
    {
        public THandler Actions { get; private set; }
        protected abstract THandler SetupHandler();

        protected void Awake()
        {
            Actions = SetupHandler();
        }

        protected void OnEnable()
        {
            Actions.Start();
        }

        protected void OnDisable()
        {
            Actions.Stop();
        }
    }
}
