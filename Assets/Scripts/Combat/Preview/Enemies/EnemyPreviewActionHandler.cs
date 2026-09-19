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

            previewTile.SetCanBeSelected(false);
            previewTile.SetMeshes(CombatManager.EnemyPreviewGroups.Movement);
            previewTile.SetColor(_stageEntityController.Info.EntityColor, _stageEntityController.Info.EntityColorHDR, _stageEntityController.Info.EntitySecondaryColor);
        }
    }
}
