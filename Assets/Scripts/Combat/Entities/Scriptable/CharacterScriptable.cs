using FMODUnity;
using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/Combat/Entity/Character")]
    public class CharacterScriptable : StageEntityScriptable
    {

        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _usedIcon;
        [Header("Stats")]
        [SerializeField] private int motivation = 100;
        [SerializeField] private CharacterController _prefab;
        [ParamRef] [SerializeField] private string _fmodParameterName;

        #region Properties


        public Sprite Icon { get { return _icon; } }
        public Sprite UsedIcon { get { return _usedIcon; } }
        public int Motivation => motivation;
        public override TeamEnum Team { get { return TeamEnum.Circus; } }
        public CharacterController Prefab { get { return _prefab; } }
        public string FmodParameterName { get { return _fmodParameterName;  } }

        #endregion
    }
}
