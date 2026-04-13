using Lugu.Singleton;
using System;
using UnityEngine;

namespace RPG
{
    public class ActionsManager : Singleton<ActionsManager>
    {
        public Action OnMapChanged;
    }
}
