using UnityEngine;

namespace LucasRozado.Utility
{
    public static class Layer
    {
        static public bool IsInMask(int layer, LayerMask mask)
        {
            int layerAsMask = 1 << (layer - 1);
            return (mask & layerAsMask) == layerAsMask;
        }
    }
}