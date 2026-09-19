using LucasRozado.Utility;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ActionPreviewMaterial : MonoBehaviour
    {
        [SerializeField] private Material material;
        private Material defaultMaterial;
        private MeshRenderer mesh;

        protected void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            defaultMaterial = mesh.materials[1];
        }
        public void StartPreview(int index)
        {
            Utility.Get(mesh).SetMaterial(material, index + 1);
        }
        public void StopPreview(int index)
        {
            Utility.Get(mesh).SetMaterial(defaultMaterial, index + 1);
        }        
    }
}
