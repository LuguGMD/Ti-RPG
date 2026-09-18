using LucasRozado.Utility;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ActionPreviewMaterial : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private Material material;
        private Material defaultMaterial;
        private MeshRenderer mesh;

        protected void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            defaultMaterial = mesh.materials[index];
        }
        public void StartPreview()
        {
            Utility.Get(mesh).SetMaterial(material, index);
        }
        public void StopPreview()
        {
            Utility.Get(mesh).SetMaterial(defaultMaterial, index);
        }        
    }
}
