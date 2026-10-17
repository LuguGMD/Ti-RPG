using RPG.Combat;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace RPG.Analytics
{
    public class CombatAnalyticsHandler : AnalyticsHandler<CombatAnalyticsEvent>
    {
        protected override void AddListeners()
        {
            ActionsManager.Instance.OnCombatWon += RecordEvent;
            ActionsManager.Instance.OnCombatLost += RecordEvent;
            ActionsManager.Instance.OnCombatStart += RegisterCharactersUsed;
        }

        protected override void RemoveListeners()
        {
            ActionsManager.Instance.OnCombatWon -= RecordEvent;
            ActionsManager.Instance.OnCombatLost -= RecordEvent;
            ActionsManager.Instance.OnCombatStart -= RegisterCharactersUsed;
        }

        private void RegisterCharactersUsed()
        {
            string[] charactersArray = new string[GameManager.CurrentParty.Length];
            string charactersSelected = "";

            for(int i = 0; i < GameManager.CurrentParty.Length; i++)
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
    }
}
