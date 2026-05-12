using UnityEngine;

using BaseUtility = LucasRozado.Utility.Utility;

namespace LucasRozado.Utility
{
    public partial class Utility
    {
        public static Unity.Layer.Utility Get(LayerMask of) => new(of);
    }

    public static partial class Unity
    {
        public static class Layer
        {
            public class Utility : BaseUtility.OfType<LayerMask>
            {
                public Utility(LayerMask of) : base(of)
                { }

                public bool HasLayer(int layer) => IsLayerInMask(layer, self);
            }

            static public bool IsLayerInMask(int layer, LayerMask mask)
            {
                int layerAsMask = 1 << (layer - 1);
                return (mask & layerAsMask) == layerAsMask;
            }

        }
    }
}
