using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using RPG.Combat.Wave;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class BossController : EnemyController
    {
        [SerializeField] private BossPushAttack _actionPush;
        [SerializeField] private BossRotateAttack _actionRotate;

        [SerializeField] private int _actionPoints;
        [SerializeField] private int _actionPointsMaximumAdd;
        [SerializeField] private int _actionPointsRamp;

        private float _defeatsPerHit = 3;
        private int _defeatsCount = 0;

        protected new void Start()
        {
            base.Start();
            _tileObject = GetComponent<TileObject>();
            _tileObject.SetCurrentTile(new Tile(new Vector2Int(0, 0), transform), true);
            _tileObject.SetEntity(this);
            CombatManager.Instance.AddEnemy(this);

            _defeatsPerHit = GameManager.BossFightLevel.TotalEnemies / 3f;
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            ActionsManager.Instance.OnEnemyDefeated += OnEnemyDefeated;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            ActionsManager.Instance.OnEnemyDefeated -= OnEnemyDefeated;
        }

        protected override void InitCombatActions()
        {
            if (_actions.Count == 0)
            {
                _actions.Add(_actionPush);
                _actions.Add(_actionRotate);
                base.InitCombatActions();
            }
        }

        private int TurnActionPoints(int turn)
        {
            int points = (int)(-1 / Mathf.Exp(turn / _actionPointsRamp)) * _actionPointsMaximumAdd;
            return -points;
        }
        public override void PrepareAction()
        {
            if (_actions.Count == 0)
            {
                InitCombatActions();
            }

            _selectedActionIndex = -1;

            int pointsToAdd = TurnActionPoints(CombatManager.TurnCount);
            _actionPoints += pointsToAdd;

            (BossCombatAction action, int index) chosen = (null, 0);
            int choicesCount = 0;
            for (int i = 0; i < _actions.Count; i++)
            {
                BossCombatAction action = _actions[i] as BossCombatAction;
                bool canBuy = action.ActionCost < _actionPoints;
                if (!canBuy)
                {
                    continue;
                }
                bool meetsRequirements = action.CheckRequirements();
                if (!meetsRequirements)
                {
                    continue;
                }
                else if (chosen.action == null)
                {
                    choicesCount = 1;
                    chosen = (action, i);
                    continue;
                }
                else if (action.ActionPriority < chosen.action.ActionPriority)
                {
                    continue;
                }
                else if (action.ActionPriority > chosen.action.ActionPriority)
                {
                    choicesCount = 1;
                    chosen = (action, i);
                    continue;
                }
                else
                {
                    choicesCount += 1;
                    float replaceChance = 1f / choicesCount;

                    bool shouldReplace = Random.value < replaceChance;
                    if (shouldReplace)
                    { chosen = (action, i); }
                }

            }

            if (chosen.action != null)
            {
                SelectAction(chosen.index);
                List<PreviewTileInfo> tiles = chosen.action.Preview();
                _preparedAction = tiles.Count > 0 ? tiles[0] : null;
            }

            ManipulateEnemiesAction();
        }

        public void ManipulateEnemiesAction()
        {
            List<EnemyController> enemies = CombatManager.RemainingEnemies;

            Dictionary<CharacterController, List<(EnemyController, ActionTargetInfo)>> targets = new();
            foreach (EnemyController enemy in enemies)
            {
                enemy.CalculateActionTargets();
                foreach (ActionTargetInfo action in enemy.ActionTargets)
                {
                    if(action.Target != null)
                    if (!targets.TryAdd(action.Target, new() { (enemy, action) }))
                    { targets[action.Target].Add((enemy, action)); }
                }
            }

            //Debug.Log("Manipulating enemies >:)");

            foreach (var target in targets)
            {
                CharacterController character = target.Key;
                List<(EnemyController, ActionTargetInfo)> actions = target.Value;

                foreach ((EnemyController enemy, ActionTargetInfo action) current in actions)
                {
                    if (!current.enemy.ActionTargets.Contains(current.action))
                    { actions.Remove(current); }
                }

                if (actions.Count > 1)
                {
                    (EnemyController enemy, ActionTargetInfo action) best = actions[0];
                    float bestValue = character.CurrentMotivation
                        - CombatConstants.MAX_MOTIVATION_APRESENTADOR
                        - CombatManager.Apresentador.CurrentMotivation;
                    foreach ((EnemyController enemy, ActionTargetInfo action) current in actions)
                    {
                        float value = current.action.Damage
                            - character.CurrentMotivation
                            - CombatConstants.MAX_MOTIVATION_APRESENTADOR
                            - CombatManager.Apresentador.CurrentMotivation;

                        bool isBestValueLethal = 0 <= bestValue;
                        bool isCurrentValueLethal = 0 <= value;

                        bool bestValueDefeats = isBestValueLethal && !isCurrentValueLethal;
                        bool bestValueMoreDamage = !isCurrentValueLethal && value < bestValue;
                        bool bestValueMoreEffective = isBestValueLethal && isCurrentValueLethal && bestValue < value;
                        if (bestValueDefeats || bestValueMoreEffective || bestValueMoreDamage)
                        {
                            current.enemy.ActionTargets.Remove(current.action);
                        }
                        else
                        {
                            var nowPreviousBest = best;
                            best.enemy.ActionTargets.Remove(nowPreviousBest.action);
                            best = current;
                            bestValue = value;
                        }
                    }

                    best.enemy.ActionTargets.Clear();
                    best.enemy.ActionTargets.Add(best.action);
                }
            }
        }

        private void OnEnemyDefeated(EnemyController enemy)
        {
            _defeatsCount++;
            if (_defeatsCount >= _defeatsPerHit)
            {
                _defeatsCount = 0;
                TakeDamage(1);
            }
        }
    }
}
