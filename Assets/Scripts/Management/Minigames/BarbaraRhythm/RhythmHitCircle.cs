using UnityEngine;

namespace RPG.Management.Minigames.Rhythm
{
    public class RhythmHitCircle : MonoBehaviour
    {

        public void TryHit()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, RhythmMinigameHandler.HIT_RADIUS);
            foreach(Collider hit in hits)
            {
                float hitDistance = Vector3.Distance(transform.position, hit.transform.position);
                if (hitDistance <= RhythmMinigameHandler.PERFECT_DISTANCE)
                {
                    ActionsManager.Instance.OnMinigamePerfectHit?.Invoke();
                }
                else
                {
                    ActionsManager.Instance.OnMinigameHit?.Invoke();
                }

                Destroy(hit.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, RhythmMinigameHandler.HIT_RADIUS);
        }
    }
}
