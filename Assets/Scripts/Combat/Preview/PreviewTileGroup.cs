using UnityEngine;

namespace RPG.Combat.Preview
{
    [CreateAssetMenu(fileName = "PreviewTileGroup", menuName = "Scriptable Objects/Combat/PreviewTileGroup")]
    public class PreviewTileGroup : ScriptableObject
    {
        [SerializeField] private PreviewTileModels _movement;
        [SerializeField] private PreviewTileModels _effect;

        #region Properties

        public PreviewTileModels Movement { get { return _movement; } }
        public PreviewTileModels Effect { get { return _effect; } }

        #endregion
    }
}
