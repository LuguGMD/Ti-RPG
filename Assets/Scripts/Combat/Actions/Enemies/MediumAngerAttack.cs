using FMODUnity;
using RPG.Audio;
using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using RPG.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class MediumAngerAttack : CombatAction
    {
        private const float shakeCameraForce = 0.25f;

        [SerializeField] private float _damage;
        [SerializeField] private EventReference _attackSFX;
        private Vector2Int _direction;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[1].Commands.Add(new DamageEffect(_damage));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            AudioManager.Instance.PlayOneShot(_attackSFX);
            yield return new WaitForSeconds(0.3f / CombatManager.CombatSpeed);

            do
            {
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);

                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);

           

            if(_effects[0].Execute(_user))
            {
                CombatManager.Instance.CameraShake(shakeCameraForce);
            }
            _effects[1].Execute(_user);
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo first;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            Effect previewEffect = Effect.Clone(_effects[0]);
            previewEffect.Area[0] = new Vector2Int(0, 0);

            first = new PreviewTileInfo(_direction, _direction.ToDirection(), false, alwaysShowEffect: true);
            first.Effects.Add(previewEffect);
            PreviewTileInfo child = first.CreateChild(_direction, _direction.ToDirection(), false, alwaysShowEffect: true);
            child.Effects.Add(previewEffect);

            firstSteps.Add(first);

            return firstSteps;
        }

        public void ChangeDirection(Vector2Int direction)
        {
            _direction = direction;
        }
    }
}
