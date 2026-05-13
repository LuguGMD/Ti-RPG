using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class CombatUIController : MonoBehaviour
    {

        //TO DO trocar para usar um panel controller para a troca de canvas ativos

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
            bool isPlayerTurn = CombatManager.CurrentTurnState == CombatTurnStateEnum.PlayerTurn;
            bool isEnemyTurn = CombatManager.CurrentTurnState == CombatTurnStateEnum.EnemyTurn;

            if (playerActionButton != null)
            {
                playerActionButton.interactable = isPlayerTurn;
                if (playerActionButtonText != null)
                {
                    playerActionButtonText.text = isPlayerTurn ? "Finalizar Turno" : "Aguarde...";
                }
            }
        }

        private string GetTurnStateName(CombatTurnStateEnum state)
        {
            return state switch
            {
                CombatTurnStateEnum.PlayerTurn => "Turno do Player",
                CombatTurnStateEnum.EnemyTurn => "Turno do Enemy",
                CombatTurnStateEnum.BattleEnd => "Fim da Batalha",
                _ => "Desconhecido"
            };
        }

        private void ShowCanvas()
        {
            CombatUIManager.Instance.ChangePanel(_mainPanel);
        }

        private void HideCanvas()
        {
            CombatUIManager.Instance.DisablePanel(_mainPanel);
        }
    }
}
