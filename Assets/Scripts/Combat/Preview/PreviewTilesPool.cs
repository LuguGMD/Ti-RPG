using Lugu.Singleton;
using RPG.Combat.Grid;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.Combat.Preview
{
    public class PreviewTilesPool : SingletonMono<PreviewTilesPool>
    {
        private static ObjectPool<ActionPreviewTile> _pool;

        #region Properties

        public static ObjectPool<ActionPreviewTile> Pool { get { return _pool; } }

        #endregion

        protected void Start()
        {
            if (Instance == this)
            {
                _pool = new ObjectPool<ActionPreviewTile>
                (
                    CombatFactory.InstantiatePreviewActionTile,
                    actionOnGet: OnGet,
                    actionOnRelease: OnRelease,
                    actionOnDestroy: OnClear,
                    collectionCheck: true,
                    defaultCapacity: 10,
                    maxSize: Map.Rows * Map.Columns

                   );
            }
        }

        private void OnGet(ActionPreviewTile previewTile)
        {
            previewTile.gameObject.SetActive(true);
        }

        private void OnRelease(ActionPreviewTile previewTile)
        {
            previewTile.gameObject.SetActive(false);
        }

        private void OnClear(ActionPreviewTile previewTile)
        {
            previewTile.gameObject.SetActive(false);
        }
    }
}
