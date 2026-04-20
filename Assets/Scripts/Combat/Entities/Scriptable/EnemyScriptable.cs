using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/Entity/Enemy")]
    public class EnemyScriptable : StageEntityScriptable
    {
        [SerializeField] private EnemyController _prefab;
        [SerializeField] private float _health;

        #region Properties

        public EnemyController Prefab { get { return _prefab; } }
        public float Health { get { return _health; } }
        public override TeamEnum Team { get { return TeamEnum.Enemies; } }

        #endregion
    }
}
