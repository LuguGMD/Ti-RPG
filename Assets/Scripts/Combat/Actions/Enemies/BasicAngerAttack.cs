using FMODUnity;
using RPG.Audio;
using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class BasicAngerAttack : CombatAction
    {
        [SerializeField] private float _damage;
        [SerializeField] private EventReference _attackSFX;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[1].Commands.Add(new DamageEffect(_damage));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);
            yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
            AudioManager.Instance.PlayOneShot(_attackSFX);
            foreach (Effect effect in _effects)
            {
                effect.Execute(_user);
            }
            yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
        }


        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo down;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            Effect previewEffect = Effect.Clone(_effects[0]);
            previewEffect.Area[0] = new Vector2Int(0, 0);

            down = new PreviewTileInfo(Vector2Int.down, Grid.DirectionEnum.Down, true, alwaysShowEffect: true);
            down.Effects.Add(previewEffect);
            PreviewTileInfo child = down.CreateChild(Vector2Int.down, Grid.DirectionEnum.Down, true, alwaysShowEffect: true);
            child.Effects.Add(previewEffect);

            //firstSteps.Add(up);
            firstSteps.Add(down);
            //firstSteps.Add(right);
            //firstSteps.Add(left);

            return firstSteps;
        }
    }
}
