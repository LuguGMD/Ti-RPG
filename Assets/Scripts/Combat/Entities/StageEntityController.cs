using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using RPG.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(TileObjectMovement), typeof(PreviewActionHandler))]
    public abstract class StageEntityController : EntityController
    {
        [SerializeField] protected StageEntityScriptable _info;

        protected TileObjectMovement _movement;
        protected PreviewActionHandler _preview;
        protected int _selectedActionIndex;

        #region Properties

        public TileObjectMovement Movement { get { return _movement; } }
        public StageEntityScriptable Info { get { return _info; } }
        public PreviewActionHandler Preview { get { return _preview; } }

        #endregion

        protected new void Awake()
        {
            base.Awake();
            _movement = GetComponent<TileObjectMovement>();
            _preview = GetComponent<PreviewActionHandler>();

            //TO DO remover depois
            SelectAction(0);
        }

        protected new void Start()
        {
            base.Start();
            AdjsutGameSpeed();
            ActionsManager.Instance.OnStageEntityCreated?.Invoke(this);
        }

        public IEnumerator UseSelectedAction(int movementPatternIndex, int repetitions, bool isMirrored)
        {
            SetAnimationInt("ActionIndex", _selectedActionIndex);
            SetAnimationBool("IsActionRunning", true);

            CombatAction action = _info.Actions[_selectedActionIndex];
            MovementPattern movementPattern = action.MovementPatterns[movementPatternIndex];

            SubscribeEffects(action.Effects);

            DirectionEnum startDirection = movementPattern.Pattern[0].Direction;
            if(isMirrored) startDirection = startDirection.Mirror();

            _tileObject.SetDirection(startDirection);

            _movement.EnqueuePattern(movementPattern.Pattern, repetitions);

            ActionsManager.Instance.OnActionStart?.Invoke();

            yield return new WaitForSeconds(0.1f / CombatManager.CombatSpeed);

            while (_movement.MovementQueue.Count > 0)
            {
                yield return _movement.Move(isMirrored);
            }

            yield return new WaitUntil(() => !IsInActionAnimation());
            _tileObject.UpdatePosition();

            ActionsManager.Instance.OnActionEnd?.Invoke();
            SetAnimationBool("IsActionRunning", false);

            yield return new WaitForSeconds(0.1f / CombatManager.CombatSpeed);

            CombatManager.UnsubscribeEffectTriggerAction();
        }

        public void SelectAction(int actionIndex)
        {
            _selectedActionIndex = actionIndex;
            _preview.ChangeActionToPreview(_info.Actions[actionIndex]);
        }

        private void SubscribeEffects(List<Effect> effects)
        {
            foreach (Effect effect in effects)
            {
                CombatManager.SubscribeEffectTriggerAction(effect.TriggerCondition, () => effect.Execute(this));
            }
        }

        protected override void Defeated()
        {
            base.Defeated();
            ActionsManager.Instance.OnStageEntityDefeated?.Invoke(this);
        }
    }
}
