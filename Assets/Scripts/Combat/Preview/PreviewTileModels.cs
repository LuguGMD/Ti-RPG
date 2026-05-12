using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "PreviewTileModels", menuName = "Scriptable Objects/Combat/PreviewTileModels")]
    public class PreviewTileModels : ScriptableObject
    {
        [SerializeField] private Mesh[] _previewLinesMeshs;

        #region Properties

        public Mesh[] PreviewLinesMeshs { get { return _previewLinesMeshs; } }

        #endregion
    }
}
