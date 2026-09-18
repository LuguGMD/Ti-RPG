using UnityEngine;

namespace RPG.Combat
{
    
    public abstract class EntityScriptable : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _entityName = "Entity";
        [SerializeField] private string _entityDescription = "Description";
        [SerializeField] private GameObject _previewModelPrefab;
        [SerializeField] private Color _entityColor;
        [SerializeField] private Color _entitySecondaryColor;

        #region Properties

        public string EntityName => _entityName;
        public string EntityDescription => _entityDescription;
        public abstract TeamEnum Team { get; }
        public GameObject PreviewModelPrefab => _previewModelPrefab;
        public Color EntityColor => _entityColor;
        public Color EntitySecondaryColor => _entitySecondaryColor;

        #endregion
    }
}
