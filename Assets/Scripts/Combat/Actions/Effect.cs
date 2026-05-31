using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using RPG.Extensions;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class Effect
    {
        private List<EffectCommand> _commands = new List<EffectCommand>();

        [SerializeField] private List<TeamEnum> _targetList = new List<TeamEnum>();
        [SerializeField] private bool _canTargetSelf = false;
        [Tooltip("If the character needs to be in the spotlight to this to activate")]
        [SerializeField] private bool _doNeedSpotlight = false;
        [SerializeField] private bool _isRelativeToMovement;
        [SerializeField] private List<Vector2Int> _area = new List<Vector2Int>();

        #region Properties

        public List<EffectCommand> Commands { get { return _commands; } }
        public List<TeamEnum> TargetList { get { return _targetList; } }
        public bool CanTargetSelf { get { return _canTargetSelf; } }
        public bool DoNeedSpotlight { get { return _doNeedSpotlight; } }
        public bool IsRelativeToMovement { get { return _isRelativeToMovement; } }
        public List<Vector2Int> Area { get { return _area; } }


        #endregion

        public void Execute(StageEntityController user)
        {
           
            List<Vector2Int> checkedTiles = new List<Vector2Int>();

            if (_doNeedSpotlight && !user.TileObject.IsOnSpotlight)
            {
                return;
            }

            for (int i = 0; i < _area.Count; i++)
            {
                Vector2Int checkPosition = user.Position;
                checkPosition += _isRelativeToMovement ? _area[i].RelativeTo(user.Direction) : _area[i];

                /*Debug.Log("User Position: " + user.Position);
                Debug.Log("User Direction: " + user.Direction);
                Debug.Log("Check Position: " + checkPosition);
                Debug.Log(_area[i]);*/

                if (checkedTiles.Contains(checkPosition)) continue;
                Tile tile = MapManager.Map.GetTile(checkPosition);

                if (tile.IsOccupied)
                {
                    EntityController entity = tile.TileObject.GetComponent<EntityController>();

                    //Debug.Log("Tile Occupied: " + entity.name);

                    if (CombatManager.CanTarget(user.Info, entity.GetEntityInfo(), this))
                    {
                        //Debug.Log("Can Target");


                        if (tile.Position == Map.CENTER_POS)
                        {
                            foreach (EffectCommand command in _commands)
                            {
                                command.ExecuteApresentador(user, entity as ApresentadorController);
                            }
                        }
                        else
                        {
                            foreach (EffectCommand command in _commands)
                            {
                                command.Execute(user, entity as StageEntityController);
                            }
                        }
                    }

                }

                checkedTiles.Add(checkPosition);
            }
        }

        public List<ActionPreviewTile> Preview(Vector2Int startPosition, DirectionEnum startDirection)
        {
            
            List<Vector2Int> checkedTiles = new List<Vector2Int>();
            List<ActionPreviewTile> actionPreviewTiles = new List<ActionPreviewTile>();

            for (int i = 0; i < _area.Count; i++)
            {
                Vector2Int checkPosition = startPosition;
                checkPosition += _isRelativeToMovement ? _area[i].RelativeTo(startDirection) : _area[i];

                if (checkedTiles.Contains(checkPosition)) continue;
                if (checkPosition.y >= Map.Rows) continue;
                Tile tile = MapManager.Map.GetTile(checkPosition);

                if (tile.Position == Map.CENTER_POS)
                {
                    //TO DO Adicionar Preview Apresentador
                }
                else
                {
                    ActionPreviewTile previewTile = AddPreviewTile(checkPosition);
                    if(previewTile != null)
                        actionPreviewTiles.Add(previewTile);
                }

                checkedTiles.Add(checkPosition);
            }

            return actionPreviewTiles;
        }

        private ActionPreviewTile AddPreviewTile(Vector2Int position)
        {
            if (position.y < 0)
            {
                return null;
            }

            PreviewTilesPool.Pool.Get(out ActionPreviewTile previewTile);
            previewTile.SetCanBeSelected(false);
            previewTile.SetPosition(position);
            previewTile.SetMeshes(CombatManager.CharacterPreviewGroups.Effect);

            //_activePreviewTiles.Add(previewTile);
            return previewTile;
        }

        public static Effect Clone(Effect original)
        {
            Effect clone = new Effect();
            clone._commands = new List<EffectCommand>();
            foreach (EffectCommand cmd in original._commands)
            {
                clone._commands.Add(cmd);
            }

            clone._targetList = new List<TeamEnum>();
            foreach (TeamEnum target in original._targetList)
            {
                clone._targetList.Add(target);
            }

            clone._canTargetSelf = original._canTargetSelf;
            clone._doNeedSpotlight = original._doNeedSpotlight;
            clone._isRelativeToMovement = original._isRelativeToMovement;

            clone._area = new List<Vector2Int>();
            foreach (Vector2Int area in original._area)
            {
                clone._area.Add(area);
            }

            return clone;
        }

    }
}
