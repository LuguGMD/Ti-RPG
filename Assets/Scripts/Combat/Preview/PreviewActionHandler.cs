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
        protected List<PreviewTile> _activePreviewTiles = new List<PreviewTile>();

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
            
            //TO DO remover depois
            CalculatePreviewTiles();
        }


        //TO DO bake de preview para apenas calcular viabilidade de tiles no futuro
        private void CalculatePreviewTiles()
        {
            _previewTileInfos.Clear();

            //TO DO adicionar preview de efeitos
            //List<EffectTrigger> _effectTriggers = _actionToPreview.GetEffectTriggers();

            List<MovementPattern> movementPatterns = _actionToPreview.MovementPatterns;

            for (int i = 0; i < movementPatterns.Count; i++)
            {
                GetPatternTiles(movementPatterns[i], i, false);
                if (movementPatterns[i].CanMirror)
                    GetPatternTiles(movementPatterns[i], i, true);
            }
        }

        public virtual void ShowPreview()
        {
            HidePreview();

            _isPreviewing = true;

            for (int i = 0; i < _previewTileInfos.Count; i++)
            {
                Vector2Int position = _previewTileInfos[i].RelativePosition;
                position += _stageEntityController.Position;

                if (IsPositionValid(_previewTileInfos[i], position))
                {
                    AddPreviewTile(_previewTileInfos[i], position);
                }
            }
        }

        private bool IsPositionValid(PreviewTileInfo previewTileInfo, Vector2Int position)
        {
            Tile tile = MapManager.Map.GetTile(position);

            if (tile.Position == Map.CENTER_POS) return false;
            if(tile.IsOccupied)
            {
                if(tile.TileObject.TryGetComponent<StageEntityController>(out StageEntityController stageEntity))
                {
                    //TO DO adicionar mais condições em relação ao ataque selecionado
                    if (stageEntity.Info.Team == _stageEntityController.Info.Team) return false;
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
            PreviewTilesPool.Pool.Get(out PreviewTile previewTile);
            previewTile.SetInfo(previewTileInfo);
            previewTile.SetCanBeSelected(true);
            previewTile.SetPosition(position);

            _activePreviewTiles.Add(previewTile);

        }

        private void GetPatternTiles(MovementPattern movementPattern, int patternIndex, bool isMirror)
        {
            Vector2Int startPos = Vector2Int.zero;

            List<Movement> pattern = movementPattern.Pattern;
            Vector2Int currentPos = startPos;

            for (int j = 0; j < movementPattern.Repetition; j++)
            {

                for (int k = 0; k < pattern.Count; k++)
                {
                    Movement movement = pattern[k];
                    Direction direction = isMirror ? movement.Direction.Mirror() : movement.Direction;
                    currentPos += direction.ToVector2Int();
                }

                _previewTileInfos.Add(new PreviewTileInfo(currentPos, patternIndex, j+1, isMirror, false, true));
            }
        }

        private void UpdatePreview()
        {
            if(_isPreviewing)
            {
                ShowPreview();
            }
        }

    }
}
