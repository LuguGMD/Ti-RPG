using Lugu.Singleton;
using RPG.Combat.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CombatManager : SingletonMono<CombatManager>
    {
        public static readonly Dictionary<CombatType, CombatType> TypeChart = new Dictionary<CombatType, CombatType>()
        {
            { CombatType.Magic, CombatType.Anger },
            { CombatType.Strength, CombatType.Fear },
            { CombatType.Jokes, CombatType.Sadness },
            { CombatType.Fear, CombatType.Magic },
            { CombatType.Sadness, CombatType.Strength },
            { CombatType.Anger, CombatType.Jokes },
        };

        public static bool IsTargetWeak(CombatType user, CombatType target)
        {
            return TypeChart[user] == target;
        }

        public static void SubscribeEffectTriggerAction(EffectTrigger effectTrigger, Action action)
        {
            switch (effectTrigger)
            {
                case EffectTrigger.ActionStart:
                    ActionsManager.Instance.OnActionStart += action;
                    break;
                case EffectTrigger.ActionEnd:
                    ActionsManager.Instance.OnActionEnd += action;
                    break;
                case EffectTrigger.PatternEnd:
                    ActionsManager.Instance.OnPatternEnd += action;
                    break;
                case EffectTrigger.BeforeTileStep:
                    ActionsManager.Instance.OnTileStepBefore += action;
                    break;
                case EffectTrigger.AfterTileStep:
                    ActionsManager.Instance.OnTileStepAfter += action;
                    break;
            }
        }
        public static void UnsubscribeEffectTriggerAction()
        {
            ActionsManager.Instance.OnActionStart = null;
            ActionsManager.Instance.OnActionEnd = null;
            ActionsManager.Instance.OnPatternEnd = null;
            ActionsManager.Instance.OnTileStepBefore = null;
            ActionsManager.Instance.OnTileStepAfter = null;
        }

        public static bool CanTarget(EntityScriptable user, EntityScriptable target, Effect effect)
        {
            if(user == target)
            {
                return effect.CanTargetSelf;
            }

            return effect.TargetList.Contains(target.Team);
        }
    }
}