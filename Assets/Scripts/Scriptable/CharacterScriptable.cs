using RPG.Combat;
using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/Entity/Character")]
    public class CharacterScriptable : StageEntityScriptable
    {

        [Header("Stats")]
        [SerializeField] private int motivation = 100;

        #region Properties

        public int Motivation => motivation;
        public override Team Team { get { return Team.Circus; } }

        #endregion
    }
}
