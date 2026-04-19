using RPG.Combat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class BattleUIController : MonoBehaviour
    {

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
    }
}
