using RPG.Combat;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "ApresentadorScriptable", menuName = "Scriptable Objects/ApresentadorScriptable")]
    public class ApresentadorScriptable : EntityScriptable
    {
        #region Properties

        public override Team Team { get { return Team.Circus; } }

        #endregion
    }
}
