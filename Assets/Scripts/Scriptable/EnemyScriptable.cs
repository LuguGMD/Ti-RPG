using RPG.Combat;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/Entity/Enemy")]
    public class EnemyScriptable : StageEntityScriptable
    {
        [SerializeField] private EnemyController _prefab;
        [SerializeField] private float _health;

        #region Properties

        public EnemyController Prefab { get { return _prefab; } }
        public float Health { get { return _health; } }
        public override Team Team { get { return Team.Enemies; } }

        #endregion
    }
}
