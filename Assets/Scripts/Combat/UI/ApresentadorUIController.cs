using DG.Tweening;
using RPG.Combat.Grid;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class ApresentadorUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _mainPanel;

        [SerializeField] private Button _changeRowUpButton;
        [SerializeField] private Button _changeRowDownButton;
        [SerializeField] private Button _rotateLeftButton;
        [SerializeField] private Button _rotateRightButton;
        [SerializeField] private Button _cancelActionButton;
        [SerializeField] private Button _confirmActionButton;
        [SerializeField] private TextMeshProUGUI _rotationsLeftText;

        private Dictionary<int, int> _rowsRotatedAmount = new Dictionary<int, int>();
        private int _rotatedAmount = 0;

        private bool _isCanvasEnabled = false;

        private void OnEnable()
        {
            ActionsManager.Instance.OnApresentadorSelected += ShowCanvas;
            ActionsManager.Instance.OnEntitySelected += OnEntitySelected;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorSelected -= ShowCanvas;
            ActionsManager.Instance.OnEntitySelected -= OnEntitySelected;
        }

        private void Start()
        {
            _changeRowDownButton.onClick.AddListener(() => ChangeRow(-1));
            _changeRowUpButton.onClick.AddListener(() => ChangeRow(1));

            _rotateLeftButton.onClick.AddListener(() => Rotate(1));
            _rotateRightButton.onClick.AddListener(() => Rotate(-1));

            _cancelActionButton.onClick.AddListener(CancelAction);
            _confirmActionButton.onClick.AddListener(ConfirmAction);

            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            for(int i =0; i < Map.Rows-1; i++)
            {
                _rowsRotatedAmount[i] = 0;
            }
        }

        private void OnEntitySelected(EntityController entitySelected)
        {
            if(_isCanvasEnabled && CombatManager.Apresentador != entitySelected)
            {
                CancelAction();
                HideCanvas();
            }
        }

        private void ShowCanvas()
        {
            _isCanvasEnabled = true;
            InitializeDictionary();
            _rotatedAmount = 0;
            _mainPanel.SetActive(true);

            UpdateText();
            SelectRow();
        }

        private void HideCanvas()
        {
            _isCanvasEnabled = false;
            _mainPanel.SetActive(false);

            DeselectRow();
        }

        private void ChangeRow(int amount)
        {
            DeselectRow();

            CombatManager.Apresentador.ChangeRow(amount);

            SelectRow();
        }

        private void Rotate(int amount)
        {
            int rowRotatedAmount = _rowsRotatedAmount[CombatManager.Apresentador.RowToRotate];
            bool isRemovingRotations = Mathf.Sign(amount) != Mathf.Sign(rowRotatedAmount) && rowRotatedAmount != 0;

            if (_rotatedAmount < CombatManager.Apresentador.MaxRowRotations || isRemovingRotations)
            {
                _rowsRotatedAmount[CombatManager.Apresentador.RowToRotate] += amount;
                _rotatedAmount += isRemovingRotations ? -Mathf.Abs(amount) : Mathf.Abs(amount);

                UpdateText();

                CombatManager.Apresentador.Rotate(amount);
            }
        }

        private void UpdateText()
        {
            _rotationsLeftText.text = $"Rotações restantes: {CombatManager.Apresentador.MaxRowRotations - _rotatedAmount}";
        }

        private void CancelAction()
        {
            for(int i = 0; i < Map.Rows-1; i++)
            {
                CombatManager.Apresentador.Rotate(i, -_rowsRotatedAmount[i]);
            }
            ActionsManager.Instance.OnApresentadorActionCanceled?.Invoke();
            HideCanvas();
        }

        private void ConfirmAction()
        {
            if (_rotatedAmount > 0)
            {
                ActionsManager.Instance.OnApresentadorActionCompleted?.Invoke();
                HideCanvas();
            }
            else
            {
                CancelAction();
            }
        }

        private void SelectRow()
        {
            GameObject selectedRow = MapManager.RowGameObjects[CombatManager.Apresentador.RowToRotate];
            selectedRow.transform.DOKill(true);
            selectedRow.transform.DOLocalMove(-Vector3.up * 0.25f, 0.5f);
        }

        private void DeselectRow()
        {
            GameObject selectedRow = MapManager.RowGameObjects[CombatManager.Apresentador.RowToRotate];
            selectedRow.transform.DOKill(true);
            selectedRow.transform.DOLocalMove(Vector3.zero, 0.25f);
        }
    }
}
