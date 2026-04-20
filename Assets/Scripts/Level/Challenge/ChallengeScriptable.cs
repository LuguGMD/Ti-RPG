using UnityEngine;

namespace RPG.Level.Challenge
{
    [CreateAssetMenu(fileName = "ChallengeScriptable", menuName = "Scriptable Objects/Level/Challenge")]
    public class ChallengeScriptable : ScriptableObject
    {
        private string _challengeID;
        private ChallengeTypeEnum _challengeType;
        private string _challengeName;
        private string _description;
        private float _targetValue;
        private int _coinsReward;

        #region Properties

        public string ChallengeID
        {
            get { return _challengeID; }
        }

        public ChallengeTypeEnum ChallengeType
        {
            get { return _challengeType; }
        }

        public string ChallengeName
        {
            get { return _challengeName; }
        }

        public string Description
        {
            get { return _description; }
        }

        public float TargetValue
        {
            get { return _targetValue; }
        }

        public int CoinsReward
        {
            get { return _coinsReward; }
        }

        #endregion
    }
}
