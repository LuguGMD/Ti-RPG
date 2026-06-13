using DG.Tweening;
using FMODUnity;
using RPG.Audio;
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
        protected List<CombatAction> _actions = new List<CombatAction>();

        protected TileObjectMovement _movement;
        protected PreviewActionHandler _preview;
        protected int _selectedActionIndex;

        [SerializeField] private EventReference _defeatedSFX;
        
        private SpotlightSparkleEffect _spotlightHandler;
        

        #region Properties

        public TileObjectMovement Movement { get { return _movement; } }
        public StageEntityScriptable Info { get { return _info; } }
        public PreviewActionHandler Preview { get { return _preview; } }
        public int SelectedActionIndex {  get { return _selectedActionIndex; } }
        public List<CombatAction> Actions => _actions;

        #endregion

        protected new void Awake()
        {
            base.Awake();
            _movement = GetComponent<TileObjectMovement>();
            _preview = GetComponent<PreviewActionHandler>();

            _tileObject.SetEntity(this);
        }

        protected new void Start()
        {
            base.Start();
            AdjsutGameSpeed();
            ActionsManager.Instance.OnStageEntityCreated?.Invoke(this);

            InitCombatActions();

            //TO DO remover depois
            SelectAction(0);
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            _spotlightHandler = GetComponent<SpotlightSparkleEffect>();
            _tileObject.OnSpotlightStateChange += _spotlightHandler.UpdateSpotlightEffect;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            _tileObject.OnSpotlightStateChange -= _spotlightHandler.UpdateSpotlightEffect;
        }
        

        public IEnumerator UseSelectedAction(PreviewTileInfo selectedPreviewTile)
        {
            _hasActed = true;
            _tileObject.CheckSpotlight();

            SetAnimationInt("ActionIndex", _selectedActionIndex);
            SetAnimationBool("IsActionRunning", true);

            CombatAction action = _actions[_selectedActionIndex];

            ActionsManager.Instance.OnActionStart?.Invoke();
            yield return action.Execute(selectedPreviewTile);

            yield return new WaitForSeconds(0.1f / CombatManager.CombatSpeed);

            SetAnimationBool("IsActionRunning", false);

            yield return new WaitUntil(() => !IsInActionAnimation());
            _tileObject.UpdatePosition();

            ActionsManager.Instance.OnActionEnd?.Invoke();
            TileObject.CheckSpotlight();
        }

        public void SelectAction(int actionIndex)
        {
            _selectedActionIndex = actionIndex;
            _preview.ChangeActionToPreview(_actions[actionIndex]);
        }

        protected override void Defeated()
        {
            base.Defeated();
            AudioManager.Instance.PlayOneShot(_defeatedSFX);
            ActionsManager.Instance.OnStageEntityDefeated?.Invoke(this);
        }

        protected virtual void InitCombatActions()
        {
            foreach (CombatAction combatAction in _actions)
            {
                combatAction.Init(this);
            }
        }
    }
}
