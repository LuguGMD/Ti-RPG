
namespace LucasRozado.Utility
{
    public delegate T Getter<T>();
    public delegate void Setter<T>(T value);

    public static partial class Utility
    {
        public static OfType<TSelf> Get<TSelf>(TSelf of) => new(of);
        public class OfType<TSelf>
        {
            protected readonly TSelf self;
            public OfType(TSelf self)
            {
                this.self = self;
            }

            public void Invoke(string methodName, params object[] parameters)
            {
                var method = typeof(TSelf).GetMethod(methodName);
                method.Invoke(self, parameters);
            }

            public TReturn Invoke<TReturn>(string methodName, params object[] parameters)
            {
                var method = typeof(TSelf).GetMethod(methodName);
                return (TReturn)method.Invoke(self, parameters);
            }
        }
    }
}