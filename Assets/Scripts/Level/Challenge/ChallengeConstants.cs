using RPG.Level.Challenge.Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Level.Challenge
{
    public class ChallengeConstants
    {
        public static Dictionary<ChallengeTypeEnum, Type> ChallengeComponents = new Dictionary<ChallengeTypeEnum, Type>()
        {
            {ChallengeTypeEnum.DefeatEnemiesInOneTurn, typeof(ChallengeHandlerDefeatEnemiesInOneTurn) },
            {ChallengeTypeEnum.MotivationLimit, typeof(ChallengeHandlerMotivationLimit) }
        };
    }
}
