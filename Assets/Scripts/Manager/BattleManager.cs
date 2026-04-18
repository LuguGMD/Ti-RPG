using UnityEngine;

namespace RPG
{
    public class BattleManager : MonoBehaviour
    {
        #region Propriedades

        [Header("Turn Settings")]
        private BattleTurnState currentTurnState;
        private int turnCount;
        private bool isBattleOver;

        #endregion

        #region Metodos Combate

        private void Start()
        {
            InitializeBattle();
        }

        private void InitializeBattle()
        {
            currentTurnState = BattleTurnState.PlayerTurn;
            turnCount = 1;
            isBattleOver = false;
        }

        #endregion Metodos Combate

        #region Metodos Player
        public void ExecutePlayerAction()
        {
            if (isBattleOver || currentTurnState != BattleTurnState.PlayerTurn)
                return;

            ExecutePlayerTurn();
            SwitchTurn();
        }

        private void ExecutePlayerTurn()
        {
            Debug.Log($"[Turno {turnCount}] Turno do Player executado!");
        }

        #endregion Metodos Player

        #region Metodos Enemy
        public void ExecuteEnemyAction()
        {
            if (isBattleOver || currentTurnState != BattleTurnState.EnemyTurn)
                return;

            ExecuteEnemyTurn();
            SwitchTurn();
        }

        private void ExecuteEnemyTurn()
        {
            Debug.Log($"[Turno {turnCount}] Turno do Enemy executado!");
        }

        private void SwitchTurn()
        {
            if (currentTurnState == BattleTurnState.PlayerTurn)
            {
                currentTurnState = BattleTurnState.EnemyTurn;
                Debug.Log($"Turno {turnCount}: Player -> Enemy");
            }
            else if (currentTurnState == BattleTurnState.EnemyTurn)
            {
                currentTurnState = BattleTurnState.PlayerTurn;
                turnCount++;
                Debug.Log($"Turno {turnCount}: Enemy -> Player");
            }
        }
        #endregion Metodos Enemy

        public BattleTurnState GetCurrentTurnState() => currentTurnState;
        public int GetTurnCount() => turnCount;

    }
}
