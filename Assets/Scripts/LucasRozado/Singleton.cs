using UnityEngine;

namespace LucasRozado.Utility
{
    public class Singleton<TSelf>
        where TSelf : new()
    {

        static public TSelf Instance => instanceGetter.Invoke();
        static public void Create(TSelf instance) => instanceSetter.Invoke(instance);

        static private TSelf instance;
        static private Getter<TSelf> instanceGetter = TryGetFirstInstance;
        static private Setter<TSelf> instanceSetter = SetFirstInstance;

        static private TSelf TryGetFirstInstance()
        {
            if (instance is MonoBehaviour) return GetNoInstance();
            if (instance is ScriptableObject) return GetNoInstance();

            return GetFirstInstance();
        }

        static private TSelf GetNoInstance()
        {
            Debug.LogError(
                $"{typeof(TSelf).Name}: " +
                "Tried getting a Singleton instance, " +
                "but none were ever created."
            );
            return default;
        }

        static private TSelf GetFirstInstance()
        {
            SetFirstInstance(new());
            return instance;
        }

        static private void SetFirstInstance(TSelf instance)
        {
            if (instance == null)
            {
                Debug.LogError(
                    $"{typeof(TSelf).Name}: " +
                    "Tried creating a null Singleton instance."
                );
                return;
            }

            Singleton<TSelf>.instance = instance;
            instanceGetter = GetInstance;
            instanceSetter = FailToSetInstance;
        }

        static private TSelf GetInstance() => instance;

        static private void FailToSetInstance(TSelf _)
        {
            Debug.LogWarning(
                $"{typeof(TSelf).Name}: " +
                "Failed to create another Singleton instance."
            );
        }
    }
}