using RPG.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG.Analytics
{
    public class CombatAnalyticsHandler : AnalyticsHandler<CombatAnalyticsEvent>
    {
        private CombatTurnData _turnData;
        private List<CombatTurnData> _turnsData = new List<CombatTurnData>();

        protected override void AddListeners()
        {
            ActionsManager.Instance.OnCombatWon += OnCombatWon;
            ActionsManager.Instance.OnCombatLost += OnCombatLost;
            ActionsManager.Instance.OnCombatStart += RegisterCharactersUsed;
            ActionsManager.Instance.OnEnemyDefeated += RegisterEnemyDefeated;
            ActionsManager.Instance.OnCharacterDefeated += RegisterCharacterDefeated;
            ActionsManager.Instance.OnApresentadorDamageTaken += RegisterApresentadorDamageTaken;
            ActionsManager.Instance.OnCharacterDamageTaken += RegisterCharacterDamageTaken;
            ActionsManager.Instance.OnEnemyDamageTaken += RegisterEnemyDamageTaken;
            ActionsManager.Instance.OnPlayerTurnStarted += SetupTurn;
            ActionsManager.Instance.OnEnemyTurnEnded += RegisterTurn;
        }

        protected override void RemoveListeners()
        {
            ActionsManager.Instance.OnCombatWon -= OnCombatWon;
            ActionsManager.Instance.OnCombatLost -= OnCombatLost;
            ActionsManager.Instance.OnCombatStart -= RegisterCharactersUsed;
            ActionsManager.Instance.OnEnemyDefeated -= RegisterEnemyDefeated;
            ActionsManager.Instance.OnCharacterDefeated -= RegisterCharacterDefeated;
            ActionsManager.Instance.OnApresentadorDamageTaken -= RegisterApresentadorDamageTaken;
            ActionsManager.Instance.OnCharacterDamageTaken -= RegisterCharacterDamageTaken;
            ActionsManager.Instance.OnEnemyDamageTaken -= RegisterEnemyDamageTaken;
            ActionsManager.Instance.OnPlayerTurnStarted -= SetupTurn;
            ActionsManager.Instance.OnEnemyTurnEnded -= RegisterTurn;
        }

        private void RegisterCharactersUsed()
        {
            string[] charactersArray = new string[GameManager.CurrentParty.Length];
            string charactersSelected = "";

            for (int i = 0; i < GameManager.CurrentParty.Length; i++)
            {
                charactersArray[i] = GameManager.CurrentParty[i].EntityName;
            }

            Array.Sort(charactersArray);
            foreach (string character in charactersArray)
            {
                charactersSelected += character + " ";
            }

            _event.CharactersSelected = charactersSelected;
        }

        private void SetupTurn()
        {
            _turnData = new CombatTurnData();
        }

        private void RegisterTurn()
        {
            _turnsData.Add(_turnData);
        }

        private void RegisterEnemyDefeated(EnemyController enemy)
        {
            _turnData.CharacterDefeated += enemy.Info.EntityName + " ";
        }

        private void RegisterCharacterDefeated(CharacterController character)
        {
            _turnData.CharacterDefeated += character.Info.EntityName + " ";
        }

        private void RegisterApresentadorDamageTaken(float amount)
        {
            _turnData.ApresentadorDamage += amount;
        }

        private void RegisterCharacterDamageTaken(CharacterController character, float amount)
        {
            _turnData.CharactersDamage += amount;
        }

        private void RegisterEnemyDamageTaken(EnemyController enemy, float amount)
        {
            _turnData.EnemiesDamage += amount;
        }

        private void OnCombatWon()
        {
            _event.IsVictory = true;
            RecordEvent();
        }

        private void OnCombatLost()
        {
            _event.IsVictory = false;
            RecordEvent();
        }

        protected override void RecordEvent()
        {
            _event.LevelName = GameManager.SelectedLevel.levelName;
            _event.CombatResume = JsonUtility.ToJson(_turnsData, true);
            base.RecordEvent();
        }
    }
}
