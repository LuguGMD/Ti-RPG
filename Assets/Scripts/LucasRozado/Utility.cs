using System;
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

        public static Utility<TSelf> GetUtils<TSelf>(TSelf of) => new(of);
        public static Collection.HashSet.Utility<TValue> GetUtils<TValue>(
            HashSet<TValue> of
        ) => new(of);


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