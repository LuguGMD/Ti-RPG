using UnityEngine;

namespace RPG.Management.Minigames.Rhythm
{
    public class RhythmNote : MonoBehaviour
    {
        private Vector3 _direction;
        private float _speed;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Init(float speed, Vector3 direction, Sprite sprite)
        {
            _direction = direction;
            _speed = speed;
            _spriteRenderer.sprite = sprite;
        }

        private void Update()
        {
            if (MinigameManager.IsPaused) return;

            transform.position += _direction * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
            ActionsManager.Instance.OnMinigameMiss?.Invoke();
        }
    }
}
