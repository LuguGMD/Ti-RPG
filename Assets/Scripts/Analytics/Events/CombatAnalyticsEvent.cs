using UnityEngine;

namespace RPG.Analytics
{
    public class CombatAnalyticsEvent : Unity.Services.Analytics.Event
    {
        public CombatAnalyticsEvent() : base("combatEnded") { }

        public string CharactersSelected { set { SetParameter("charactersSelected", value); } }
    }
}
