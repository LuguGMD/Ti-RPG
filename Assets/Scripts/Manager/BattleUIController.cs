using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class BattleUIController : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;

        [Header("Player UI")]
        [SerializeField] private Button playerActionButton;
        [SerializeField] private Text playerActionButtonText;

        [Header("Enemy UI")]
        [SerializeField] private Button enemyActionButton;
        [SerializeField] private Text enemyActionButtonText;

        [Header("Battle Info")]
        [SerializeField] private Text turnInfoText;

        private void Start()
        {
            SetupUI();
            UpdateUI();
        }

        private void SetupUI()
        {
            if (playerActionButton != null)
            {
                playerActionButton.onClick.AddListener(OnPlayerActionClicked);
            }

            if (enemyActionButton != null)
            {
                enemyActionButton.onClick.AddListener(OnEnemyActionClicked);
            }
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (turnInfoText != null)
            {
                var turnState = battleManager.GetCurrentTurnState();
                turnInfoText.text = $"Turno {battleManager.GetTurnCount()} - {GetTurnStateName(turnState)}";
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool isPlayerTurn = battleManager.GetCurrentTurnState() == BattleTurnState.PlayerTurn;
            bool isEnemyTurn = battleManager.GetCurrentTurnState() == BattleTurnState.EnemyTurn;

            if (playerActionButton != null)
            {
                playerActionButton.interactable = isPlayerTurn;
                if (playerActionButtonText != null)
                {
                    playerActionButtonText.text = isPlayerTurn ? "Ação Player" : "Aguarde...";
                }
            }

            if (enemyActionButton != null)
            {
                enemyActionButton.interactable = isEnemyTurn;
                if (enemyActionButtonText != null)
                {
                    enemyActionButtonText.text = isEnemyTurn ? "Ação Inimigo" : "Aguarde...";
                }
            }
        }

        private void OnPlayerActionClicked()
        {
            if (battleManager != null)
            {
                battleManager.ExecutePlayerAction();
            }
        }

        private void OnEnemyActionClicked()
        {
            if (battleManager != null)
            {
                battleManager.ExecuteEnemyAction();
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
