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
        [SerializeField] private EffectTrigger _triggerCondition = EffectTrigger.ActionEnd;
        [SerializeField] private List<Team> _targetList = new List<Team>();
        [SerializeField] private bool _canTargetSelf = false;
        [Tooltip("If the character needs to be in the spotlight to this to activate")]
        [SerializeField] private bool _doNeedSpotlight = false;

        [Header("Area Of Effect")]
        [SerializeField] private bool _isRelativeToMovement = true;
        [SerializeField] private List<Direction> _area = new List<Direction>();

        #region Properties

        public EffectCommandScriptable Command { get { return _command; } }

        public EffectTrigger TriggerCondition { get { return _triggerCondition; } }
        public List<Team> TargetList { get { return _targetList; } }
        public bool CanTargetSelf { get { return _canTargetSelf; } }
        public bool DoNeedSpotlight { get { return _doNeedSpotlight; }  }

        public bool IsRelativeToMovement { get { return _isRelativeToMovement; } }
        public List<Direction> Area { get { return _area; } }
        

        #endregion

        public void Execute(StageEntityController user)
        {
            Vector2Int checkPosition = user.Position;
            List<Vector2Int> checkedTiles = new List<Vector2Int>();

            for (int i = 0; i < _area.Count; i++)
            {
                Direction direction = _isRelativeToMovement ? _area[i].RelativeTo(user.Direction) : _area[i];
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
