using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "ApresentadorScriptable", menuName = "Scriptable Objects/ApresentadorScriptable")]
    public class ApresentadorScriptable : EntityScriptable
    {
        #region Properties

        public override TeamEnum Team { get { return TeamEnum.Circus; } }

        #endregion
    }
}
