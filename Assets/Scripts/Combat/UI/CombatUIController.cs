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

        [SerializeField] private Button _speedButton;

        [Header("Battle Info")]
        [SerializeField] private TextMeshProUGUI turnInfoText;

        private void Start()
        {
            SetupUI();
            UpdateUI();
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
            ActionsManager.Instance.OnActionStart += DisableSpeedButton;
            ActionsManager.Instance.OnActionEnd += EnableSpeedButton;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorActionCanceled -= ShowCanvas;
            ActionsManager.Instance.OnApresentadorActionCompleted -= ShowCanvas;
            ActionsManager.Instance.OnApresentadorSelected -= HideCanvas;
            ActionsManager.Instance.OnActionStart -= DisableSpeedButton;
            ActionsManager.Instance.OnActionEnd -= EnableSpeedButton;
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

        private void UpdateUI()
        {
            if (turnInfoText != null)
            {
                var turnState = CombatManager.CurrentTurnState;
                turnInfoText.text = $"Turno {CombatManager.TurnCount}";
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool isPlayerTurn = CombatManager.CurrentTurnState == CombatTurnStateEnum.PlayerTurn;
            bool isEnemyTurn = CombatManager.CurrentTurnState == CombatTurnStateEnum.EnemyTurn;
            bool isPlayerActing = CombatManager.IsActionInProgress;

            if (playerActionButton != null)
            {
                playerActionButton.interactable = isPlayerTurn && !isPlayerActing;
            }
        }

        private string GetTurnStateName(CombatTurnStateEnum state)
        {
            return state switch
            {
                CombatTurnStateEnum.PlayerTurn => "Turno do Jogador",
                CombatTurnStateEnum.EnemyTurn => "Turno do Inimigo",
                CombatTurnStateEnum.BattleEnd => "Fim da Batalha",
                _ => "Desconhecido"
            };
        }

        private void EnableSpeedButton()
        {
            _speedButton.interactable = true;
        }

        private void DisableSpeedButton()
        {
            _speedButton.interactable = false;
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
