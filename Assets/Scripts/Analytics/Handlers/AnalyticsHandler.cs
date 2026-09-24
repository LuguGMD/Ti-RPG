using System;
using Unity.Services.Analytics;
using UnityEngine;
using Event = Unity.Services.Analytics.Event;

namespace RPG.Analytics
{
    public abstract class AnalyticsHandler<T> : MonoBehaviour where T: Event
    {
        protected T _event;

        protected virtual void Awake()
        {
            _event = (T)Activator.CreateInstance(typeof(T));
        }

        protected virtual void OnEnable()
        {
            AddListeners();
        }

        protected virtual void OnDisable()
        {
            RemoveListeners();
        }

        protected abstract void AddListeners();
        protected abstract void RemoveListeners();
        protected virtual void RecordEvent()
        {
            if (!AnalyticsManager.Instance.HasConsented) return;
            
            AnalyticsService.Instance.RecordEvent(_event);
        }
    }
}
