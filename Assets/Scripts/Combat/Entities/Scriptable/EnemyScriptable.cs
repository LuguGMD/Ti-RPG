using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/Combat/Entity/Enemy")]
    public class EnemyScriptable : StageEntityScriptable
    {
        [SerializeField] private EnemyController _prefab;
        [SerializeField] private float _health;

        [Header("Tooltip")]
        [TextArea]
        [SerializeField] private string _combatDescription;

        #region Properties

        public EnemyController Prefab { get { return _prefab; } }
        public float Health { get { return _health; } }
        public string CombatDescription { get { return _combatDescription; } }
        public override TeamEnum Team { get { return TeamEnum.Enemies; } }

        #endregion
    }
}