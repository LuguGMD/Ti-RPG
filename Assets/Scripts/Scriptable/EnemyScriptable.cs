using RPG.Combat;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/Entity/Enemy")]
    public class EnemyScriptable : StageEntityScriptable
    {
        [SerializeField] private EnemyController _prefab;

        #region Properties

        public EnemyController Prefab { get { return _prefab; } }
        public override Team Team { get { return Team.Enemies; } }

        #endregion
    }
}
