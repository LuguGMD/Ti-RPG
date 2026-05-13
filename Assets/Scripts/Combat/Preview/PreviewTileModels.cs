using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "PreviewTileModels", menuName = "Scriptable Objects/Combat/PreviewTileModels")]
    public class PreviewTileModels : ScriptableObject
    {
        [SerializeField] private Mesh[] _previewLinesMeshs;
        [SerializeField] private Material _material;

        #region Properties

        public Mesh[] PreviewLinesMeshs { get { return _previewLinesMeshs; } }
        public Material Material { get { return _material; } }

        #endregion
    }
}
