using UnityEngine;

namespace RPG.Combat.Preview
{
    public class EnemyPreviewActionHandler : PreviewActionHandler
    {
        //TO DO adicionar logica da IA do inimigo para qual opção escolher

        protected override void AddPreviewTile(PreviewTileInfo previewTileInfo, Vector2Int position, ref ActionPreviewTile lastPreviewTile, bool canBeSelected = false)
        {
            base.AddPreviewTile(previewTileInfo, position, ref lastPreviewTile);

            if (_activePreviewTiles.Count == 0) return;

            ActionPreviewTile previewTile = _activePreviewTiles[_activePreviewTiles.Count - 1];

            previewTile.SetCanBeSelected(canBeSelected);
            previewTile.SetMeshes(CombatManager.EnemyPreviewGroups.Effect);
        }
    }
}
