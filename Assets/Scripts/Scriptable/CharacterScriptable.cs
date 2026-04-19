using RPG.Combat;
using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/Entity/Character")]
    public class CharacterScriptable : StageEntityScriptable
    {

        [Header("Stats")]
        [SerializeField] private int motivation = 100;
        [SerializeField] private CharacterController _prefab;

        #region Properties

        public int Motivation => motivation;
        public override Team Team { get { return Team.Circus; } }
        public CharacterController Prefab { get { return _prefab; } }

        #endregion
    }
}
