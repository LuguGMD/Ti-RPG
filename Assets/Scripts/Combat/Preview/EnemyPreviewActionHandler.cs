using UnityEngine;

namespace RPG.Combat.Preview
{
    public class EnemyPreviewActionHandler : PreviewActionHandler
    {
        //TO DO adicionar logica da IA do inimigo para qual opção escolher

        protected override void AddPreviewTile(PreviewTileInfo previewTileInfo, Vector2Int position)
        {
            base.AddPreviewTile(previewTileInfo, position);

            ActionPreviewTile previewTile = _activePreviewTiles[_activePreviewTiles.Count - 1];

            previewTile.SetCanBeSelected(false);
            previewTile.SetMaterial(CombatManager.EnemyPreviewMaterial);
        }
    }
}
