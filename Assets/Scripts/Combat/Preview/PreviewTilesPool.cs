using Lugu.Singleton;
using RPG.Combat.Grid;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.Combat.Preview
{
    public class PreviewTilesPool : SingletonMono<PreviewTilesPool>
    {
        private static ObjectPool<PreviewTile> _pool;

        #region Properties

        public static ObjectPool<PreviewTile> Pool { get { return _pool; } }

        #endregion

        protected void Start()
        {
            if (Instance == this)
            {
                _pool = new ObjectPool<PreviewTile>
                (
                    CombatFactory.InstantiatePreviewTile,
                    actionOnGet: OnGet,
                    actionOnRelease: OnRelease,
                    actionOnDestroy: OnClear,
                    collectionCheck: true,
                    defaultCapacity: 10,
                    maxSize: Map.Rows * Map.Columns

                   );
            }
        }

        private void OnGet(PreviewTile previewTile)
        {
            
        }

        private void OnRelease(PreviewTile previewTile)
        {
            
        }

        private void OnClear(PreviewTile previewTile)
        {
        }
    }
}
