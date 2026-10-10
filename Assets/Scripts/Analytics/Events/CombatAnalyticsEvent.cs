using UnityEngine;

namespace RPG.Analytics
{
    public class CombatAnalyticsEvent : Unity.Services.Analytics.Event
    {
        public CombatAnalyticsEvent() : base("combatEnded") { }

        public string CharactersSelected { set { SetParameter("charactersSelected", value); } }
        public bool IsVictory { set { SetParameter("isVictory", value); } }
        public string CombatResume { set { SetParameter("CombatResume", value); } }
        public string LevelName { set { SetParameter("levelName", value); } }
    }
}
