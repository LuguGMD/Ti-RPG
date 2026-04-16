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
        [SerializeField] private bool _canAffectAllies = false;
        [SerializeField] private bool _canAffectFoes = true;
        [SerializeField] private bool _canAffectSelf = false;
        [Tooltip("If the character needs to be in the spotlight to this to activate")]
        [SerializeField] private bool _doNeedSpotlight = false;

        [Header("Area Of Effect")]
        [SerializeField] private bool _isRelativeToMovement = true;
        [SerializeField] private List<Direction> _area = new List<Direction>();

        #region Properties

        public EffectCommandScriptable Command { get { return _command; } }

        public EffectTrigger TriggerCondition { get { return _triggerCondition; } }
        public bool CanAffectAllies { get { return _canAffectAllies; } }
        public bool CanAffectFoes { get { return _canAffectFoes; } }
        public bool CanAffectSelf { get { return _canAffectSelf; } }
        public bool DoNeedSpotlight { get { return _doNeedSpotlight; }  }

        public bool IsRelativeToMovement { get { return _isRelativeToMovement; } }
        public List<Direction> Area { get { return _area; } }
        

        #endregion

        public void Execute(StageEntityController user)
        {
            Vector2Int checkPosition = user.Position;

            for (int i = 0; i < _area.Count; i++)
            {
                Direction direction = _isRelativeToMovement ? _area[i].RelativeTo(user.Direction) : _area[i];

                checkPosition += direction.ToVector2Int();
                Tile tile = MapManager.Instance.Map.GetTile(checkPosition);

                //TO DO check if can target this target
                if (tile.Position == Map.CENTER_POS)
                {
                    _command.ExecuteApresentador(user, tile.TileObject.GetComponent<ApresentadorController>());
                }
                else if (tile.IsOccupied)
                {
                    _command.Execute(user, tile.TileObject.GetComponent<StageEntityController>());
                }
            }
        }

    }
}
