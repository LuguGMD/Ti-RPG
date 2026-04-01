using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG
{
    public sealed class InputManager
    {
        static public InputSystem_Actions Actions { get; } = new();
        static private Dictionary<Type, InputActionMap> ActionMaps { get; } = new();

        static public void Register(InputActionMap actionMap, Type actionsType)
        {
            if (!ActionMaps.ContainsKey(actionsType))
            {
                ActionMaps[actionsType] = actionMap;
                actionMap.Enable();
            }
        }

        static public void Enable<TActions>()
        {
            Type actionsType = typeof(TActions);
            if (ActionMaps.TryGetValue(actionsType, out InputActionMap actionMap))
            { actionMap.Enable(); }

        }
        static public void Disable<TActions>()
        {
            Type actionsType = typeof(TActions);
            if (ActionMaps.TryGetValue(actionsType, out InputActionMap actionMap))
            { actionMap.Disable(); }
        }
    }
    public abstract class InputManager<TActions, THandler> : MonoBehaviour
        where TActions : struct
        where THandler : class
    {
        protected abstract InputActionMap ActionMap { get; }
        
        protected abstract InputHandler<TActions, THandler> Handler { get; }

        protected void Awake()
        {
            Type actionsType = typeof(TActions);
            InputManager.Register(ActionMap, actionsType);
        }

        protected void OnEnable()
        { Handler.Start(); }

        protected void OnDisable()
        { Handler.Stop(); }
    }
}
