using UnityEngine;
using UnityEngine.Playables;

namespace RPG
{
    public class CityAnimationTrigger : MonoBehaviour
    {
        PlayableDirector timeline;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            timeline = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                timeline.Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Invoke("ResetAnimation", 2);
            }
        }

        void ResetAnimation()
        {
            timeline.time = 0;
            timeline.Evaluate();
            timeline.Stop();
        }
    }
}
