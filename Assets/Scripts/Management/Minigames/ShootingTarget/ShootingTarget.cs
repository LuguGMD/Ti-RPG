using DG.Tweening;
using UnityEngine;

namespace RPG.Management.Minigames.Shooting
{
    public class ShootingTarget : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 2f;
        [SerializeField] private float _perfectDistance = 0.2f;

        private float _timeLeft;
        private bool _resolved = false;

        public float PerfectDistance { get { return _perfectDistance; } }

        private void Start()
        {
            _timeLeft = _lifetime;

            transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack).From(Vector3.zero);
        }

        private void Update()
        {
            if (MinigameManager.IsPaused) return;

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0f)
            {
                Miss();
            }
        }

        public void TryHit(Vector3 hitPoint)
        {
            if (_resolved) return;
            _resolved = true;

            float hitDistance = Vector3.Distance(transform.position, hitPoint);
            if (hitDistance <= _perfectDistance)
            {
                ActionsManager.Instance.OnMinigamePerfectHit?.Invoke();
            }
            else
            {
                ActionsManager.Instance.OnMinigameHit?.Invoke();
            }

            Destroy(gameObject);
        }

        private void Miss()
        {
            if (_resolved) return;
            _resolved = true;

            ActionsManager.Instance.OnMinigameMiss?.Invoke();
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _perfectDistance);
        }
    }
}
