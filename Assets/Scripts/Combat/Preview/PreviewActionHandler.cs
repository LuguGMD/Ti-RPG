using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace RPG.Combat.Preview
{
    [RequireComponent(typeof(StageEntityController))]
    public class PreviewActionHandler : MonoBehaviour
    {
        protected StageEntityController _stageEntityController;

        protected CombatAction _actionToPreview;

        protected List<PreviewTileInfo> _previewTileInfos = new List<PreviewTileInfo>();
        protected List<ActionPreviewTile> _activePreviewTiles = new List<ActionPreviewTile>();

        private bool _isPreviewing = false;

        protected void Awake()
        {
            _stageEntityController = GetComponent<StageEntityController>();
        }

        protected void OnEnable()
        {
            ActionsManager.Instance.OnMapChanged += UpdatePreview;
        }

        protected void OnDisable()
        {
            ActionsManager.Instance.OnMapChanged -= UpdatePreview;
        }

        public void ChangeActionToPreview(CombatAction actionToPreview)
        {
            _actionToPreview = actionToPreview;

            _previewTileInfos = _actionToPreview.Preview();

            //TO DO remover depois
            if (_isPreviewing)
                ShowPreview();
        }

        public virtual void ShowPreview()
        {
            HidePreview();

            _isPreviewing = true;

            for (int i = 0; i < _previewTileInfos.Count; i++)
            {
                PreviewTileInfo currentPreviewTileInfo = _previewTileInfos[i];
                Vector2Int position = _stageEntityController.Position;
                bool doCancelPattern = false;
                do
                {
                    position += currentPreviewTileInfo.RelativePosition;

                    if (IsPositionValid(currentPreviewTileInfo, position, out doCancelPattern))
                    {
                        AddPreviewTile(currentPreviewTileInfo, position);
                    }
                    else if (!_actionToPreview.LastTileNeedsToBeEmpty)
                    {
                        AddPreviewTile(currentPreviewTileInfo, position);
                        doCancelPattern = true;
                    }

                    currentPreviewTileInfo = currentPreviewTileInfo.Child;
                } while (currentPreviewTileInfo != null && !doCancelPattern);
            }
        }

        private bool IsPositionValid(PreviewTileInfo previewTileInfo, Vector2Int position, out bool doCancelPattern)
        {
            doCancelPattern = false;
            if (position.y >= Map.Rows) return false;

            Tile tile = MapManager.Map.GetTile(position);

            if (tile.Position == Map.CENTER_POS) return false;
            if (tile.IsOccupied)
            {
                if (tile.TileObject.TryGetComponent<StageEntityController>(out StageEntityController stageEntity))
                {
                    doCancelPattern = previewTileInfo.NeedsToBeEmpty;
                    //TO DO adicionar mais condições em relação ao ataque selecionado
                    return false;
                }
            }

            return true;
        }

        public void HidePreview()
        {
            _isPreviewing = false;

            for (int i = 0; i < _activePreviewTiles.Count; i++)
            {
                PreviewTilesPool.Pool.Release(_activePreviewTiles[i]);
            }

            _activePreviewTiles.Clear();
        }

        protected virtual void AddPreviewTile(PreviewTileInfo previewTileInfo, Vector2Int position)
        {
            if (position.y < 0)
            {
                return;
            }

            PreviewTilesPool.Pool.Get(out ActionPreviewTile previewTile);
            previewTile.SetInfo(previewTileInfo);
            previewTile.SetCanBeSelected(true);
            previewTile.SetPosition(position);
            previewTile.SetMeshes(CombatManager.CharacterPreviewGroups.Movement);

            _activePreviewTiles.Add(previewTile);

        }

        private void UpdatePreview()
        {
            if (_isPreviewing)
            {
                ShowPreview();
            }
        }

    }
}
