using RPG.Combat;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/Entity/Enemy")]
    public class EnemyScriptable : StageEntityScriptable
    {
        public override Team Team { get { return Team.Enemies; } }
    }
}
