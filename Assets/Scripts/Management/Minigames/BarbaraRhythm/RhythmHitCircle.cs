using DG.Tweening;
using UnityEngine;

namespace RPG.Management.Minigames.Rhythm
{
    public class RhythmHitCircle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _visual;
        [SerializeField] private SpriteRenderer _keyPreview;

        public void TryHit()
        {
            _keyPreview.enabled = false;
            transform.DOKill(true);

            transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.InBack).From(Vector3.one).OnComplete(()=>
            {
                transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            });

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
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, RhythmMinigameHandler.HIT_RADIUS);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, RhythmMinigameHandler.PERFECT_DISTANCE);
        }
    }
}
