using System.Collections.Generic;

namespace LucasRozado.Utility
{
    public delegate T Getter<T>();
    public delegate void Setter<T>(T value);

    public static class Utility
    {
        public static OfType<TSelf> Get<TSelf>(TSelf of) => new(of);
        public static Collection.HashSet.Utility<TValue> Get<TValue>(
            HashSet<TValue> of
        ) => new(of);


        public class OfType<TSelf>
        {
            protected readonly TSelf self;
            public OfType(TSelf self)
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
                public class Utility<T> : OfType<HashSet<T>>
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