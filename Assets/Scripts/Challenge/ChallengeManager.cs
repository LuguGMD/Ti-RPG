using System.Collections.Generic;
using UnityEngine;

namespace RPG.Challenge
{
    public class ChallengeManager : MonoBehaviour
    {
        private List<ChallengeHandler> _challengeHandlers;

        private void OnDestroy()
        {
            UnsubscribeChallenges();
        }

        private void LoadChallenges()
        {
            ChallengeScriptable[] challenges = new ChallengeScriptable[3]/*TO DO - GameManager.Instance.CurrentLevel.Challenges*/;
            for (int i = 0; i < challenges.Length; i++)
            {
                //TO DO - Get from save later
                bool isCompleted = false;

                if (!isCompleted)
                {
                    AddChallenge(challenges[i].ChallengeType);
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
                    //TO DO - Send to save later
                    //TO DO - ActionsManager.OnChallengeCompleted?.Invoke(_challengeHandlers[i].Info);
                }
            }
        }

        private void AddChallenge(ChallengeType challengeType)
        {
            if (ChallengeConstants.ChallengeComponents.ContainsKey(challengeType))
            {
                Component challenge = gameObject.AddComponent(ChallengeConstants.ChallengeComponents[challengeType]);
                _challengeHandlers.Add(challenge as ChallengeHandler);
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
