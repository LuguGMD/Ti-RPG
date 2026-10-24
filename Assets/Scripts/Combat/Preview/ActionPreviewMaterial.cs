using LucasRozado.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ActionPreviewMaterial : MonoBehaviour
    {
        [SerializeField] private Material material;
        private Material defaultMaterial;
        private MeshRenderer mesh;
        private Dictionary<int, Coroutine> _activeCoroutines = new Dictionary<int, Coroutine>();

        protected void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            defaultMaterial = mesh.materials[1];
        }
        public void StartPreview(int index)
        {
            if (!_activeCoroutines.ContainsKey(index))
            {
                _activeCoroutines.Add(index, StartCoroutine(MaterialChange(index)));
            }
        }
        public void StopPreview(int index)
        {
            if (_activeCoroutines.ContainsKey(index))
            {
                StopCoroutine(_activeCoroutines[index]);
                ChangeMaterial(defaultMaterial, index + 1);
                _activeCoroutines.Remove(index);
            }
        }

        private IEnumerator MaterialChange(int index)
        {
            while (true)
            {
                ChangeMaterial(material, index + 1);
                yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));
                ChangeMaterial(defaultMaterial, index + 1);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void ChangeMaterial(Material material, int index)
        {
            Material[] materials = mesh.materials;
            materials[index] = material;
            mesh.SetMaterials(materials.ToList());
        }
    }
}
