using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{

    public class CharacterController : StageEntityController
    {

        private float _currentMotivation;

        [Header("Motivation Bar")]
        [SerializeField] private Canvas _damageBarCanvas;
        [SerializeField] private Slider _damageSlider;
        [SerializeField] private float _damageSliderOffset = 2f; 

        private float _accumulatedDamage = 0f;
        private float _maxHostMotivation = 100f;
        private RectTransform _damageBarRectTransform;
        private CanvasGroup _damageBarCanvasGroup;

        public override EntityScriptable GetEntityInfo()
        {
            return _info;
        }
        
        protected new void Awake()
        {
            base.Awake();
            InitializeDamageBar();
        }

        protected new void Start()
        {
            base.Start();
            CharacterScriptable info = (CharacterScriptable)_info;
            _currentMotivation = info.Motivation;

            UIManager uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                var charMotivationBar = uiManager.CharacterMotivationBar;
                if (charMotivationBar != null)
                {
                    charMotivationBar.AddCharacter(GetHashCode(), gameObject.name);
                }
            }
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            ActionsManager.Instance.OnApresentadorDamageTaken += CheckDefeated;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            ActionsManager.Instance.OnApresentadorDamageTaken -= CheckDefeated;
        }

        private void InitializeDamageBar()
        {
            if (_damageSlider == null)
            {
                Debug.LogWarning($"CharacterController InitializeDamageBar {gameObject.name}: Slider de dano n�o foi atribu�do no inspetor!");
                return;
            }

            _damageBarRectTransform = _damageSlider.GetComponent<RectTransform>();
            _damageBarCanvasGroup = _damageSlider.GetComponent<CanvasGroup>();

            if (_damageBarCanvasGroup == null)
            {
                _damageBarCanvasGroup = _damageSlider.gameObject.AddComponent<CanvasGroup>();
            }

            _damageSlider.maxValue = 100f;
            _damageSlider.value = 0f;
            _damageSlider.interactable = false;
            _damageBarCanvasGroup.alpha = 0f;

            Debug.Log($"[CharacterController] {gameObject.name}: Barra de dano inicializada com sucesso.");
        }

        public override void TakeDamage(float damage)
        {

            _currentMotivation -= damage;

            CheckDefeated();

            Debug.Log(Info.name + " lost " + damage + " motivation\n Current Motivation: " + _currentMotivation);
        }

        private void CheckDefeated()
        {
            if (CombatManager.Apresentador.CurrentMotivation < CombatConstants.MAX_MOTIVATION_APRESENTADOR - _currentMotivation)
            {
                Defeated();
            }
        }

        protected override void Defeated()
        {
            CombatManager.Instance.RemoveCharacter(this);
            _tileObject.CurrentTile.SetTileObject(null);

            //TO DO triggar animacao de morte
            Destroy(gameObject);
        }

        public override void TakeDamage(float damage)
        {
            _motivation -= damage;
            _accumulatedDamage += damage;

            UIManager uiManager = UIManager.Instance;
            if (uiManager != null && uiManager.HostMotivationBar != null)
            {
                _maxHostMotivation = uiManager.HostMotivationBar.MaxMotivation;
            }

            UpdateLocalDamageBar();

            if (uiManager != null)
            {
                uiManager.UpdateCharacterMotivationBar(GetHashCode(), damage, _maxHostMotivation);
            }

            Debug.Log(Info.name + " lost " + damage + " motivation\n Current Motivation: " + _motivation +
                $"\nDano Acumulado: {_accumulatedDamage}/{_maxHostMotivation}");

            if (_accumulatedDamage > _maxHostMotivation)
            {
                OnCharacterDefeated();
            }
        }

        private void UpdateLocalDamageBar()
        {
            if (_damageSlider == null)
                return;

            _damageSlider.maxValue = _maxHostMotivation;
            _damageSlider.value = _accumulatedDamage;

            if (_damageBarCanvasGroup != null)
            {
                _damageBarCanvasGroup.alpha = _accumulatedDamage > 0 ? 1f : 0f;
            }
        }

        private void LateUpdate()
        {
            UpdateDamageBarPosition();
        }

        private void UpdateDamageBarPosition()
        {
            if (_damageBarRectTransform == null || _damageBarCanvas == null)
                return;

            Vector3 worldPosition = transform.position + Vector3.up * _damageSliderOffset;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _damageBarCanvas.GetComponent<RectTransform>(),
                RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition),
                _damageBarCanvas.worldCamera,
                out Vector2 canvasPosition
            );

            _damageBarRectTransform.anchoredPosition = canvasPosition;
        }

        private void OnCharacterDefeated()
        {
            Debug.LogWarning($"[CharacterController] {gameObject.name} foi derrotado! Dano: {_accumulatedDamage}/{_maxHostMotivation}");

            enabled = false;
        }

        public void ResetDamage()
        {
            _accumulatedDamage = 0f;
            UpdateLocalDamageBar();
            Debug.Log($"[CharacterController] {gameObject.name}: Dano foi resetado.");
        }

        public float GetAccumulatedDamage()
        {
            return _accumulatedDamage;
        }

        public bool IsDefeated()
        {
            return _accumulatedDamage > _maxHostMotivation;

        }

        protected override void OnSelected()
        {
            base.OnSelected();
            ActionsManager.Instance.OnCharacterSelected?.Invoke(this);
        }
    }
}
