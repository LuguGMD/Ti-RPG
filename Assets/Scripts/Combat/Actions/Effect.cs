using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class Effect
    {
        [SerializeField] private EffectCommandScriptable _command;

        [Header("Conditions")]
        [SerializeField] private EffectTriggerEnum _triggerCondition = EffectTriggerEnum.ActionEnd;
        [SerializeField] private List<TeamEnum> _targetList = new List<TeamEnum>();
        [SerializeField] private bool _canTargetSelf = false;
        [Tooltip("If the character needs to be in the spotlight to this to activate")]
        [SerializeField] private bool _doNeedSpotlight = false;

        [Header("Area Of Effect")]
        [SerializeField] private bool _isRelativeToMovement = true;
        [SerializeField] private List<DirectionEnum> _area = new List<DirectionEnum>();

        #region Properties

        public EffectCommandScriptable Command { get { return _command; } }

        public EffectTriggerEnum TriggerCondition { get { return _triggerCondition; } }
        public List<TeamEnum> TargetList { get { return _targetList; } }
        public bool CanTargetSelf { get { return _canTargetSelf; } }
        public bool DoNeedSpotlight { get { return _doNeedSpotlight; }  }

        public bool IsRelativeToMovement { get { return _isRelativeToMovement; } }
        public List<DirectionEnum> Area { get { return _area; } }
        

        #endregion

        public void Execute(StageEntityController user)
        {
            Vector2Int checkPosition = user.Position;
            List<Vector2Int> checkedTiles = new List<Vector2Int>();
            
            if(_doNeedSpotlight && !user.TileObject.IsOnSpotlight)
            {
                return;
            }

            for (int i = 0; i < _area.Count; i++)
            {
                DirectionEnum direction = _isRelativeToMovement ? _area[i].RelativeTo(user.Direction) : _area[i];
                checkPosition += direction.ToVector2Int();

                if (checkedTiles.Contains(checkPosition)) continue;

                Tile tile = MapManager.Map.GetTile(checkPosition);
                checkedTiles.Add(checkPosition);

                if (tile.IsOccupied)
                {
                    EntityController entity = tile.TileObject.GetComponent<EntityController>();

                    if (CombatManager.CanTarget(user.Info, entity.GetEntityInfo(), this))
                    {
                        if (tile.Position == Map.CENTER_POS)
                        {
                            _command.ExecuteApresentador(user, entity as ApresentadorController);
                        }
                        else
                        {
                            _command.Execute(user, entity as StageEntityController);
                        }
                    }
                }
            }
        }

    }
}
