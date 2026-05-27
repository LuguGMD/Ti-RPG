using RPG.Combat.Actions;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class StageEntityScriptable : EntityScriptable
    {
        [SerializeField] private string _spotlightDescription;

        #region Properties
        public string SpotlightDescription => _spotlightDescription;

        #endregion
    }
}
