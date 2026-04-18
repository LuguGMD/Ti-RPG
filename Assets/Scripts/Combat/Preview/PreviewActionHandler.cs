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
        private StageEntityController _stageEntityController;

        private CombatAction _actionToPreview;

        private List<PreviewTileInfo> _previewTileInfos = new List<PreviewTileInfo>();
        private List<PreviewTile> _activePreviewTiles = new List<PreviewTile>();

        protected void Awake()
        {
            _stageEntityController = GetComponent<StageEntityController>();
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

        public void ShowPreview()
        {
            HidePreview();

            for (int i = 0; i < _previewTileInfos.Count; i++)
            {
                Vector2Int position = _previewTileInfos[i].RelativePosition;
                position += _stageEntityController.Position;

                if (MapManager.IsPositionValid(position))
                {
                    AddPreviewTile(_previewTileInfos[i], position);
                }
            }
        }

        public void HidePreview()
        {
            for(int i = 0; i < _activePreviewTiles.Count; i++)
            {
                PreviewTilesPool.Pool.Release(_activePreviewTiles[i]);
            }

            _activePreviewTiles.Clear();
        }

        private void AddPreviewTile(PreviewTileInfo previewTileInfo, Vector2Int position)
        {
            PreviewTilesPool.Pool.Get(out PreviewTile previewTile);
            previewTile.SetInfo(previewTileInfo);
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

                _previewTileInfos.Add(new PreviewTileInfo(currentPos, patternIndex, j, false, false, true));
            }
        }


    }
}
