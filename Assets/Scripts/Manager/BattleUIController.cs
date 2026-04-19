using RPG.Combat;
using RPG.Combat.Grid;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class BattleUIController : MonoBehaviour
    {

        [SerializeField] private GameObject _mainPanel;

        [Header("Player UI")]
        [SerializeField] private Button playerActionButton;
        [SerializeField] private TextMeshProUGUI playerActionButtonText;

        [Header("Battle Info")]
        [SerializeField] private TextMeshProUGUI turnInfoText;

        private void Start()
        {
            SetupUI();
            UpdateUI();
        }

        private void SetupUI()
        {
            if (playerActionButton != null)
            {
                playerActionButton.onClick.AddListener(OnEndTurnClicked);
            }
        }

        private void OnEndTurnClicked()
        {
            ActionsManager.Instance.OnPlayerTurnEnded?.Invoke();
        }

        private void Update()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnApresentadorActionCanceled += ShowCanvas;
            ActionsManager.Instance.OnApresentadorActionCompleted += ShowCanvas;
            ActionsManager.Instance.OnApresentadorSelected += HideCanvas;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorActionCanceled -= ShowCanvas;
            ActionsManager.Instance.OnApresentadorActionCompleted -= ShowCanvas;
            ActionsManager.Instance.OnApresentadorSelected -= HideCanvas;
        }

        private void UpdateUI()
        {
            if (turnInfoText != null)
            {
                var turnState = CombatManager.CurrentTurnState;
                turnInfoText.text = $"Turno {CombatManager.TurnCount} - {GetTurnStateName(turnState)}";
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool isPlayerTurn = CombatManager.CurrentTurnState == BattleTurnState.PlayerTurn;
            bool isEnemyTurn = CombatManager.CurrentTurnState == BattleTurnState.EnemyTurn;

            if (playerActionButton != null)
            {
                playerActionButton.interactable = isPlayerTurn;
                if (playerActionButtonText != null)
                {
                    playerActionButtonText.text = isPlayerTurn ? "Finalizar Turno" : "Aguarde...";
                }
            }
        }

        private string GetTurnStateName(BattleTurnState state)
        {
            return state switch
            {
                BattleTurnState.PlayerTurn => "Turno do Player",
                BattleTurnState.EnemyTurn => "Turno do Enemy",
                BattleTurnState.BattleEnd => "Fim da Batalha",
                _ => "Desconhecido"
            };
        }

        private void ShowCanvas()
        {
            _mainPanel.SetActive(true);
        }

        private void HideCanvas()
        {
            _mainPanel.SetActive(false);
        }
    }
}
