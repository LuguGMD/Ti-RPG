using UnityEngine;

namespace RPG.Combat.Challenge
{
    [CreateAssetMenu(fileName = "ChallengeScriptable", menuName = "Scriptable Objects/Level/Challenge")]
    public class ChallengeScriptable : ScriptableObject
    {
        [SerializeField] private ChallengeTypeEnum _challengeType;
        [SerializeField] private string _challengeName;
        [SerializeField] private float _targetValue;
        [SerializeField] private int _coinsReward;

        #region Properties
        public ChallengeTypeEnum ChallengeType
        {
            get { return _challengeType; }
        }

        public string ChallengeName
        {
            get { return _challengeName; }
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
