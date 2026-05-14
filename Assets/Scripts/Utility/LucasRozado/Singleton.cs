using UnityEngine;

namespace LucasRozado.Utility
{
    public class Singleton<TSelf>
        where TSelf : new()
    {

        static public TSelf Instance => instanceGetter.Invoke();
        static public bool Create(TSelf instance) {
            bool hasInstance = instanceGetter == GetInstance;
            if (hasInstance)
            {
                Debug.LogWarning(
                    $"{typeof(TSelf).Name}: " +
                    "Tried to create a second Singleton instance."
                );
                return false;
            }
            else
            {
                SetInstance(instance);
                return true;
            }
        }

        static private TSelf instance;
        static private Getter<TSelf> instanceGetter = TryGetFirstInstance;

        static private TSelf TryGetFirstInstance()
        {
            switch (instance)
            {
                case MonoBehaviour:
                case ScriptableObject:
                    instanceGetter = GetNoInstance;
                    return GetNoInstance();
            }

            return GetFirstInstance();
        }

        static private TSelf GetNoInstance()
        {
            Debug.LogError(
                $"{typeof(TSelf).Name}: " +
                "Tried to get a Singleton instance, " +
                "but none were ever created."
            );
            return default;
        }

        static private TSelf GetFirstInstance()
        {
            SetInstance(new());
            return instance;
        }

        static private void SetInstance(TSelf instance)
        {
            if (instance == null)
            {
                Debug.LogError(
                    $"{typeof(TSelf).Name}: " +
                    "Tried to create a null Singleton instance."
                );
                return;
            }

            Singleton<TSelf>.instance = instance;
            instanceGetter = GetInstance;
        }

        static private TSelf GetInstance() => instance;
    }
}