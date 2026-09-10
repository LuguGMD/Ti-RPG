using System.Collections.Generic;
using RPG.Combat.Actions;
using RPG.Combat.Preview;
using UnityEngine;

namespace RPG.Combat
{
    public class BossController : EnemyController
    {

        // TODO: Criar as actions extendendo [BossCombatAction]
        [SerializeField] private BossCombatAction _action1Example;
        [SerializeField] private BossCombatAction _action2Example;

        [SerializeField] private int _actionPoints;
        [SerializeField] private int _actionPointsMaximumAdd;
        [SerializeField] private int _actionPointsRamp;

        protected override void InitCombatActions()
        {
            if (_actions.Count == 0)
            {
                _actions.Add(_action1Example);
                _actions.Add(_action2Example);
                base.InitCombatActions();
            }
        }

        private int TurnActionPoints(int turn)
        {
            int points = (int)(-1 / Mathf.Exp(turn / _actionPointsRamp)) * _actionPointsMaximumAdd;
            return points;
        }
        public override void PrepareAction()
        {
            if (_actions.Count == 0)
            {
                InitCombatActions();
            }

            int pointsToAdd = TurnActionPoints(CombatManager.TurnCount);
            _actionPoints += pointsToAdd;

            (BossCombatAction action, int index) chosen = (null, 0);
            int contenderCount = 0;
            for (int i = 0; i < _actions.Count; i++)
            {
                BossCombatAction action = _actions[i] as BossCombatAction;

                bool canBuy = action.ActionCost < _actionPoints;
                if (!canBuy) continue;

                bool meetsRequirements = action.CheckRequirements();
                if (!meetsRequirements) continue;

                if (chosen.action == null)
                {
                    contenderCount = 1;
                    chosen = (action, i);
                    continue;
                }

                if (action.ActionPriority < chosen.action.ActionPriority)
                { continue; }

                else if (action.ActionPriority > chosen.action.ActionPriority)
                {
                    contenderCount = 1;
                    chosen = (action, i);
                    continue;
                }

                else
                {
                    contenderCount += 1;
                    float replaceChance = 1f / contenderCount;

                    bool shouldReplace = Random.value < replaceChance;
                    if (shouldReplace)
                    { chosen = (action, i); }
                }

            }

            if (chosen.action != null)
            {
                SelectAction(chosen.index);
                List<PreviewTileInfo> tiles = chosen.action.Preview();
                _preparedAction = tiles[0];
            }
        }
    }
}
