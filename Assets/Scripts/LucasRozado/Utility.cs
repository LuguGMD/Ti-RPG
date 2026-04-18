using System.Collections.Generic;
using UnityEngine;

namespace LucasRozado.Utility
{
    public delegate T Getter<T>();
    public delegate void Setter<T>(T value);

    public interface ISimpleProcess
    {
        void Start();
        void Stop();
    }


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

    public static class Layer
    {
        static public bool IsInMask(int layer, LayerMask mask)
        {
            int layerAsMask = 1 << (layer - 1);
            return (mask & layerAsMask) == layerAsMask;
        }
    }

    public static class Object
    {

        public static Utility<TSelf> GetUtils<TSelf>(TSelf of) => new(of);
        public static Collection.HashSet.Utility<TValue> GetUtils<TValue>(
            HashSet<TValue> of
        ) => new(of);


        public class Utility<TSelf>
        {
            protected readonly TSelf self;
            public Utility(TSelf self)
            {
                this.self = self;
            }

            public void Invoke(string methodName, params object[] parameters)
            {
                var method = self.GetType().GetMethod(methodName);
                method.Invoke(self, parameters);
            }

            public TReturn Invoke<TReturn>(string methodName, params object[] parameters)
            {
                var method = self.GetType().GetMethod(methodName);
                return (TReturn)method.Invoke(self, parameters);
            }
        }


        public static class Collection
        {
            public static class HashSet
            {
                public class Utility<T> : Object.Utility<HashSet<T>>
                {
                    public Utility(HashSet<T> of) : base(of)
                    { }

                    public T GetAny()
                    {
                        var enumerator = self.GetEnumerator();
                        if (enumerator.Current == null)
                        { enumerator.MoveNext(); }
                        return enumerator.Current;
                    }
                }
            }
        }
    }
}