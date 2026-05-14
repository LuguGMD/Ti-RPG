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

                public bool HasLayer(int layerID) => IsLayerInMask(layerID, self);

                public LayerMask Inverse => Not(self);
            }

            static public LayerMask LayerAsMask(int layerID)
            {
                int layerAsMask = 1 << (layerID - 1);
                return layerAsMask;
            }

            static public bool IsLayerInMask(int layerID, LayerMask mask)
            {
                int layerAsMask = LayerAsMask(layerID);
                return (mask & layerAsMask) == layerAsMask;
            }

            static public LayerMask Not(LayerMask mask)
            {
                return ~mask;
            }
        }
    }
}
