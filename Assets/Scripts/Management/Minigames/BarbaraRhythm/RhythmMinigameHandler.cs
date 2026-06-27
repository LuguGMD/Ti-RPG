using RPG.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Management.Minigames.Rhythm
{
    public class RhythmMinigameHandler : MonoBehaviour
    {
        [SerializeField] private RhythmNote _notePrefab;
        [SerializeField] private List<Sprite> _noteSprites;
        [SerializeField] private List<GameObject> _noteSpawnPositions;
        [SerializeField] private List<RhythmHitCircle> _hitCircles;
        [SerializeField] private GameObject _center;

        [SerializeField] private List<float> _tiersCooldowns;
        [SerializeField] private List<float> _tiersSpeed;

        public const float HIT_RADIUS = 0.4f;
        public const float PERFECT_DISTANCE = 0.2f;

        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        }

        private void Start()
        {
            _playerInput.Actions.Move.OnUpdate(HandleInput);
            StartCoroutine(RunMinigame());
        }

        private void OnDestroy()
        {
            StopCoroutine(RunMinigame());
        }

        private void HandleInput(Vector2 input)
        {
            if(input.x > 0.1f)
            {
                _hitCircles[0].TryHit();
            }

            if (input.x < -0.1f)
            {
                _hitCircles[1].TryHit();
            }

            if (input.y > 0.1f)
            {
                _hitCircles[2].TryHit();
            }
        }


        private IEnumerator RunMinigame()
        {
            while(true)
            {
                float nextSpawnTime = _tiersCooldowns[MinigameManager.CurrentTier];
                while (nextSpawnTime > 0)
                {
                    if (!MinigameManager.IsPaused)
                    {
                        nextSpawnTime -= Time.deltaTime;
                    }

                    yield return null;
                }
                //TO DO adicionar logica de padroes de notas
                int index = Random.Range(0, _noteSpawnPositions.Count);
                InstantiateNote(index);

                yield return null;
            }
        }

        private void InstantiateNote(int index)
        {
            Vector3 spawnPosition = _noteSpawnPositions[index].transform.position;
            RhythmNote note = Instantiate<RhythmNote>(_notePrefab, spawnPosition, Quaternion.identity);
            Vector3 direction = _center.transform.position - spawnPosition;
            int spriteIndex = Random.Range(0, _noteSprites.Count);
            note.Init(_tiersSpeed[MinigameManager.CurrentTier], direction, _noteSprites[spriteIndex]);
        }
        
    }
}
