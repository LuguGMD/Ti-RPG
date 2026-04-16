using UnityEngine;

namespace RPG
{
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/Entity/Character")]
    public class CharacterScriptable : StageEntityScriptable
    {

        [Header("Stats")]
        [SerializeField] private int motivation = 100;

        
        public int Motivation => motivation;
    }
}
