using UnityEngine;

using UnityMeshRenderer = UnityEngine.MeshRenderer;
using BaseUtility = LucasRozado.Utility.Utility;

namespace LucasRozado.Utility
{
    public partial class Utility
    {
        public static Unity.MeshRenderer.Utility Get(UnityMeshRenderer of) => new(of);
    }

    public static partial class Unity
    {
        public static class MeshRenderer
        {
            public class Utility : BaseUtility.OfType<UnityMeshRenderer>
            {
                public Utility(UnityMeshRenderer of) : base(of)
                { }

                public void SetMaterial(Material material, int index)
                {
                    Material[] materials = self.materials;
                    materials[index] = material;
                    self.materials = materials;
                }
            }
        }
    }
}
