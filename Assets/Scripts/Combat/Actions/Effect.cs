using RPG.Combat.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class Effect
    {
        //Effect Function TO DO

        [Header("Conditions")]
        [SerializeField] private EffectTrigger _triggerCondition = EffectTrigger.ActionEnd;
        [SerializeField] private bool _canAffectAllies = false;
        [SerializeField] private bool _canAffectFoes = true;
        [SerializeField] private bool _canAffectSelf = false;
        [Tooltip("If the character needs to be in the spotlight to this to activate")]
        [SerializeField] private bool _doNeedSpotlight = false;

        [Header("Area Of Effect")]
        [SerializeField] private bool _isRelativeToMovement = true;
        [SerializeField] private List<Direction> _area = new List<Direction>();
    }
}
