using System.Collections.Generic;

using BaseUtility = LucasRozado.Utility.Utility;

namespace LucasRozado.Utility
{
    public static partial class Utility
    {
        public static Collections.HashSet.Utility<TValue> Get<TValue>(HashSet<TValue> of) => new(of);
    }
    
    public static partial class Collections
    {
        public static class HashSet
        {
            public class Utility<T> : BaseUtility.OfType<HashSet<T>>
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
