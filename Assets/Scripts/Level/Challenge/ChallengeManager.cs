using System.Collections.Generic;
using UnityEngine;

namespace RPG.Level.Challenge
{
    public class ChallengeManager : MonoBehaviour
    {
        private List<ChallengeHandler> _challengeHandlers = new List<ChallengeHandler>();

        private void Start()
        {
            LoadChallenges();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnCombatWon += SaveChallenges;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnCombatWon -= SaveChallenges;
        }

        private void OnDestroy()
        {
            UnsubscribeChallenges();
        }

        private void LoadChallenges()
        {
            ChallengeScriptable[] challenges = GameManager.SelectedLevel.Challenges;
            for (int i = 0; i < challenges.Length; i++)
            {
                //TO DO - Get from save later 
                //Challenge ID = $"{LevelNumber} + {i}"
                bool isCompleted = false;

                if (!isCompleted)
                {
                    AddChallenge(challenges[i].ChallengeType, challenges[i]);
                }
            }

            SubscribeChallenges();
        }
        private void SaveChallenges()
        {
            for (int i = 0; i < _challengeHandlers.Count; i++)
            {
                if (_challengeHandlers[i].IsComplete)
                {
                    int reward = _challengeHandlers[i].Info.CoinsReward;
                    GameManager.Instance.AddCoins(reward);
                    //TO DO - Send to save later
                    //Challenge ID = $"{LevelNumber} + {i}"
                }
            }
        }

        private void AddChallenge(ChallengeTypeEnum challengeType, ChallengeScriptable challengeInfo)
        {
            if (ChallengeConstants.ChallengeComponents.ContainsKey(challengeType))
            {
                ChallengeHandler challenge = gameObject.AddComponent(ChallengeConstants.ChallengeComponents[challengeType]) as ChallengeHandler;
                challenge.SetInfo(challengeInfo);
                _challengeHandlers.Add(challenge);
            }
            else
            {
                Debug.LogWarning(challengeType + " not implemented or not added to ChallengeConstants");
            }
        }

        private void SubscribeChallenges()
        {
            for (int i = 0; i < _challengeHandlers.Count; i++)
            {
                _challengeHandlers[i].SubscribeCallbacks();
            }
        }

        private void UnsubscribeChallenges()
        {
            for (int i = 0; i < _challengeHandlers.Count; i++)
            {
                _challengeHandlers[i].UnsubscribeCallbacks();
            }
        }
    }
}
