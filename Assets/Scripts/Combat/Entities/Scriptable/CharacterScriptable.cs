using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/Combat/Entity/Character")]
    public class CharacterScriptable : StageEntityScriptable
    {

        [Header("Stats")]
        [SerializeField] private int motivation = 100;
        [SerializeField] private CharacterController _prefab;

        #region Properties

        public int Motivation => motivation;
        public override TeamEnum Team { get { return TeamEnum.Circus; } }
        public CharacterController Prefab { get { return _prefab; } }

        #endregion
    }
}
