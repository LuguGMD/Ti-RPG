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
    public class MediumSadnessRegularAttack : CombatAction
    {
        [SerializeField] private float _damage;
        [SerializeField] private EventReference _attackSFX;
        private Vector2Int _direction;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[0].Commands.Add(new PushEffect(Grid.DirectionEnum.Up, 1));
            _effects[1].Commands.Add(new DamageEffect(_damage));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            _user.TileObject.SetDirection(selectedPreviewTile.Direction);

            AudioManager.Instance.PlayOneShot(_attackSFX);
            yield return new WaitForSeconds(0.3f / CombatManager.CombatSpeed);

            foreach (Effect effect in _effects)
            {
                effect.Execute(_user);
            }

            yield return _user.Movement.Move(new Movement(selectedPreviewTile.Direction, true), 1);
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo first;
            PreviewTileInfo second;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            Effect previewEffect = Effect.Clone(_effects[0]);
            previewEffect.Area[0] = new Vector2Int(0, 0);

            first = new PreviewTileInfo(_direction, _direction.ToDirection(), false, alwaysShowEffect: true);
            first.Effects.Add(previewEffect);
            second = new PreviewTileInfo(-_direction, (-_direction).ToDirection(), false, alwaysShowEffect: true);
            second.Effects.Add(previewEffect);


            firstSteps.Add(first);
            firstSteps.Add(second);

            return firstSteps;
        }

        public void ChangeDirection(Vector2Int direction)
        {
            _direction = direction;
        }
    }
}
