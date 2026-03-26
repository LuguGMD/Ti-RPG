using UnityEngine;

namespace RPG
{
    public class BattleManager : MonoBehaviour
    {
        [Header("Character Types")]
        [SerializeField] private CharacterScriptable playerType;
        [SerializeField] private CharacterScriptable enemyType;

        [Header("Instance")]
        [SerializeField] private CharacterInstance player;
        [SerializeField] private CharacterInstance enemy;

        [Header("Turn Settings")]
        private BattleTurnState currentTurnState;
        private int turnCount;
        private bool battleEnded;

        private void Start()
        {
            InitializeBattle();
        }

        private void InitializeBattle()
        {
            currentTurnState = BattleTurnState.PlayerTurn;
            turnCount = 1;
            battleEnded = false;
        }

        private void Update()
        {
            if (battleEnded)
                return;
        }

        private void ExecuteTurn()
        {
            switch (currentTurnState)
            {
                case BattleTurnState.PlayerTurn:
                    ExecutePlayerTurn();
                    break;
                case BattleTurnState.EnemyTurn:
                    ExecuteEnemyTurn();
                    break;
            }

            CheckBattleEnd();

            if (!battleEnded)
            {
                SwitchTurn();
            }
        }

        private void ExecutePlayerTurn()
        {
            
        }

        private void ExecuteEnemyTurn()
        {
           
        }

        private void SwitchTurn()
        {
          
        }

        private void CheckBattleEnd()
        {
           
        }

        public BattleTurnState GetCurrentTurnState() => currentTurnState;
        public CharacterInstance GetPlayer() => player;
        public CharacterInstance GetEnemy() => enemy;
        public bool IsBattleEnded() => battleEnded;
        public int GetTurnCount() => turnCount;
    }
}
