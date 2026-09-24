using DG.Tweening;
using RPG.Combat.Actions;
using RPG.Combat.Grid;
using RPG.Combat.Upgrades;
using RPG.Management.Progression;
using RPG.UI.Tooltip;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class ApresentadorUIController : MonoBehaviour
    {
        private const float shakeCameraForce = 0.2f;

        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _rowSelector;
        [SerializeField] private Button _rotateLeftButton;
        [SerializeField] private Button _rotateRightButton;
        [SerializeField] private Button _cancelActionButton;
        [SerializeField] private Button _confirmActionButton;
        [SerializeField] private TextMeshProUGUI _rotationsLeftText;
        [SerializeField] private Image _superBarFill;
        [SerializeField] private Button _superButton;

        private Dictionary<int, int> _rowsRotatedAmount = new Dictionary<int, int>();
        private int _rotatedAmount = 0;
        private int _confirmedRotatedAmount = 0;
        private List<int> _linesStuck = new List<int>();

        private bool _isCanvasEnabled = false;
        private bool _isSuperButtonInitialized = false;

        private void OnEnable()
        {
            ActionsManager.Instance.OnApresentadorSelected += ShowCanvas;
            ActionsManager.Instance.OnEntitySelected += OnEntitySelected;
            ActionsManager.Instance.OnTurnPassed += ResetValues;
            ActionsManager.Instance.OnEnemyTurnStarted += FreeLines;
            ActionsManager.Instance.OnMapLineStuck += StuckLine;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorSelected -= ShowCanvas;
            ActionsManager.Instance.OnEntitySelected -= OnEntitySelected;
            ActionsManager.Instance.OnTurnPassed -= ResetValues;
            ActionsManager.Instance.OnEnemyTurnStarted -= FreeLines;
            ActionsManager.Instance.OnMapLineStuck -= StuckLine;
        }

        private void Start()
        {
            _rotateLeftButton.onClick.AddListener(() => Rotate(1));
            _rotateRightButton.onClick.AddListener(() => Rotate(-1));

            _cancelActionButton.onClick.AddListener(CancelAction);
            _confirmActionButton.onClick.AddListener(ConfirmAction);

            _superButton.onClick.AddListener(UseSuper);

            InitializeDictionary();
        }

        private void ResetValues()
        {
            _confirmedRotatedAmount = 0;
        }

        private void InitializeDictionary()
        {
            for (int i = 0; i < Map.Rows - 1; i++)
            {
                _rowsRotatedAmount[i] = 0;
            }
        }

        private void OnEntitySelected(EntityController entitySelected)
        {
            if (_isCanvasEnabled && CombatManager.Apresentador.name != entitySelected.name)
            {
                ConfirmAction();
            }
        }

        private void ShowCanvas()
        {
            UpdateSuperUI();
            ActionsManager.Instance.OnApresentadorUIOpen?.Invoke();
            CombatUIManager.Instance.ChangePanel(_mainPanel);
            _rowSelector.SetActive(true);

            if (_isCanvasEnabled) return;
            InitializeDictionary();
            _rotatedAmount = 0;
            UpdateText();
            SelectRow();

            _isCanvasEnabled = true;
        }

        private void HideCanvas()
        {
            if (!_isCanvasEnabled) return;

            ActionsManager.Instance.OnApresentadorUIClose?.Invoke();

            _isCanvasEnabled = false;
            CombatUIManager.Instance.DisablePanel(_mainPanel);
            _rowSelector.SetActive(false);

            DeselectRow();
        }

        public void ChangeRow(int amount)
        {
            DeselectRow();

            CombatManager.Apresentador.ChangeRow(amount);

            SelectRow();
        }

        private void Rotate(int amount)
        {
            ActionsManager.Instance.OnRowRotated?.Invoke();

            int rowIndex = CombatManager.Apresentador.RowToRotate;
            if (_linesStuck.Contains(rowIndex))
            {
                CombatManager.Instance.CameraShake(shakeCameraForce);
                return;
            }

            int rowRotatedAmount = _rowsRotatedAmount[rowIndex];
            bool isRemovingRotations = Mathf.Sign(amount) != Mathf.Sign(rowRotatedAmount) && rowRotatedAmount != 0;

            if (_rotatedAmount + _confirmedRotatedAmount < CombatManager.Apresentador.MaxRowRotations || isRemovingRotations)
            {
                _rowsRotatedAmount[CombatManager.Apresentador.RowToRotate] += amount;
                _rotatedAmount += isRemovingRotations ? -Mathf.Abs(amount) : Mathf.Abs(amount);

                UpdateText();

                CombatManager.Apresentador.Rotate(amount);
                if(isRemovingRotations)
                {
                    CombatManager.Apresentador.ChargeSuper(-Mathf.Abs(amount));
                }
                else
                {
                    CombatManager.Apresentador.ChargeSuper(Mathf.Abs(amount));
                }
            }

            UpdateSuperUI();
        }

        private void StuckLine(int lineIndex)
        {
            _linesStuck.Add(lineIndex);
        }

        private void FreeLines()
        {
            _linesStuck.Clear();
        }

        private void UpdateText()
        {
            _rotationsLeftText.text = $"Rotações restantes: {CombatManager.Apresentador.MaxRowRotations - (_rotatedAmount + _confirmedRotatedAmount)}";
        }

        private void CancelAction()
        {
            for (int i = 0; i < Map.Rows - 1; i++)
            {
                CombatManager.Apresentador.Rotate(i, -_rowsRotatedAmount[i]);
                CombatManager.Apresentador.ChargeSuper(-Mathf.Abs(_rowsRotatedAmount[i]));
            }
            ActionsManager.Instance.OnApresentadorActionCanceled?.Invoke();
            UpdateSuperUI();
            HideCanvas();
        }

        private void ConfirmAction()
        {
            if (_rotatedAmount > 0)
            {
                CompleteAction();
            }
            else
            {
                CancelAction();
            }
        }

        private void CompleteAction()
        {
            _confirmedRotatedAmount += _rotatedAmount;

            if (_confirmedRotatedAmount >= CombatManager.Apresentador.MaxRowRotations)
            {
                CombatManager.Apresentador.CompleteAction();
                ActionsManager.Instance.OnApresentadorActionCompleted?.Invoke();
            }

            HideCanvas();
        }

        private void SelectRow()
        {
            GameObject selectedRow = MapManager.RowGameObjects[CombatManager.Apresentador.RowToRotate];
            selectedRow.transform.DOKill(true);
            selectedRow.transform.DOLocalMove(Vector3.up * 0.4f, 0.5f);
        }

        private void DeselectRow()
        {
            GameObject selectedRow = MapManager.RowGameObjects[CombatManager.Apresentador.RowToRotate];
            selectedRow.transform.DOKill(true);
            selectedRow.transform.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
            {
                ActionsManager.Instance.OnMapChanged?.Invoke();
            });
        }

        private void UpdateSuperUI()
        {
            InitSuperButton();

            if (!_superButton.gameObject.activeSelf) return;

            float amount = (float)CombatManager.Apresentador.SuperCharge / (float)CombatManager.Apresentador.EquippedSuper.ChargeAmount;
            amount = Mathf.Clamp01(amount);
            _superBarFill.DOKill(true);
            _superBarFill.DOFillAmount(amount, 0.5f);

            _superButton.interactable = amount >= 1;
        }

        private void InitSuperButton()
        {
            if(!_isSuperButtonInitialized)
            {
                Sprite buttonSprite = null;
                CombatUpgradeScriptable info = null;

                switch (GameManager.CurrentSuperEqquiped)
                {
                    case UpgradeConstants.UpgradeKey.SuperSpotlight1:
                    case UpgradeConstants.UpgradeKey.SuperSpotlight2:
                    case UpgradeConstants.UpgradeKey.SuperSpotlight3:
                        info = CombatManager.SuperSpotlightInfo;
                        buttonSprite = info.UpgradeIcon;
                        break;
                    case UpgradeConstants.UpgradeKey.SuperHeal1:
                    case UpgradeConstants.UpgradeKey.SuperHeal2:
                    case UpgradeConstants.UpgradeKey.SuperHeal3:
                        info = CombatManager.SuperHealInfo;
                        buttonSprite = info.UpgradeIcon;
                        break;
                    case UpgradeConstants.UpgradeKey.SuperPush1:
                    case UpgradeConstants.UpgradeKey.SuperPush2:
                    case UpgradeConstants.UpgradeKey.SuperPush3:
                        info = CombatManager.SuperPushInfo;
                        buttonSprite = info.UpgradeIcon;
                        break;
                }

                if (buttonSprite == null)
                {
                    _superButton.gameObject.SetActive(false);
                    _superBarFill.transform.parent.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    _superButton.GetComponent<Image>().sprite = buttonSprite;
                    _superButton.GetComponent<TooltipTrigger>().content = info.UpgradeName;
                    _superButton.GetComponent<TooltipTrigger>().header = "";
                }

                _isSuperButtonInitialized = true;
            }
        }

        private void UseSuper()
        {
            if (CombatManager.Apresentador.UseSuper())
            {
                UpdateSuperUI();
                CompleteAction();
            }
        }
    }
}
