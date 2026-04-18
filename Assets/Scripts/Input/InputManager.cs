using System;
using System.Collections.Generic;
using UnityEngine;

using LucasRozado.Utility;

namespace RPG
{
    public sealed class InputManager
    {
        static public readonly InputSystem_Actions Actions = new();
        static private readonly Dictionary<Type, HashSet<ActionsHandler>> handlers = new();
        static public IReadOnlyDictionary<Type, HashSet<ActionsHandler>> Handlers => handlers;

        static public void Register<TActions>(ActionsHandler<TActions> inputHandler)
            where TActions : struct
        {
            Type actionsType = typeof(TActions);
            if (!Handlers.ContainsKey(actionsType))
            { handlers.Add(actionsType, new()); }

            bool isSuccess = Handlers[actionsType].Add(inputHandler);
            if (!isSuccess)
            { Debug.LogWarning("Tentativa falha de inserção."); }
            else if (isSuccess && Handlers[actionsType].Count == 1)
            { StartHandling<TActions>(); }
        }

        static public void Unregister<TActions>(ActionsHandler<TActions> inputHandler)
            where TActions : struct
        {
            Type actionsType = typeof(TActions);
            if (!Handlers.ContainsKey(actionsType))
            { throw new UnityException(); }

            bool isSuccess = Handlers[actionsType].Remove(inputHandler);
            if (!isSuccess)
            { Debug.LogWarning("Tentativa falha de remoção."); }
            else if (Handlers[actionsType].Count == 0)
            { StopHandling<TActions>(); }
        }

        static private TActions GetActions<TActions>()
            where TActions : struct
        {
            Type actionsType = typeof(TActions);
            if (!Handlers.ContainsKey(actionsType))
            { throw new UnityException(); }

            if (Handlers[actionsType].Count == 0)
            { throw new UnityException(); }

            ActionsHandler<TActions> randomInputHandler
            = Utility.Get(Handlers[actionsType]).GetAny() as ActionsHandler<TActions>;

            return randomInputHandler.InputActions;
        }

        static public void StartHandling<TActions>()
            where TActions : struct
        {
            TActions inputActions = GetActions<TActions>();
            Utility.Get(inputActions).Invoke("Enable");
        }

        static public void StopHandling<TActions>()
            where TActions : struct
        {
            TActions inputActions = GetActions<TActions>();
            Utility.Get(inputActions).Invoke("Disable");
        }
    }
}
