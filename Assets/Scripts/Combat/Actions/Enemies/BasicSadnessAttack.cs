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
    public class BasicSadnessAttack : CombatAction
    {
        [SerializeField] private float _damage;
        [SerializeField] private int _pushAmount;
        [SerializeField] private EventReference _attackSFX;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[1].Commands.Add(new DamageEffect(_damage));
            _effects[0].Commands.Add(new PushEffect(Grid.DirectionEnum.Up, _pushAmount));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            do
            {
                AudioManager.Instance.PlayOneShot(_attackSFX);
                yield return new WaitForSeconds(0.9f / CombatManager.CombatSpeed);
                foreach (Effect effect in _effects)
                {
                    effect.Execute(_user);
                }
                yield return new WaitForSeconds(0.3f / CombatManager.CombatSpeed);
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);

                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo up;
            PreviewTileInfo down;
            PreviewTileInfo right;
            PreviewTileInfo left;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            Effect previewEffect = Effect.Clone(_effects[0]);

            up = new PreviewTileInfo(Vector2Int.up, Grid.DirectionEnum.Up, true);
            up.Effects.Add(previewEffect);

            down = new PreviewTileInfo(Vector2Int.down, Grid.DirectionEnum.Down, true);
            down.Effects.Add(previewEffect);


            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, true);
            right.Effects.Add(previewEffect);


            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, true);
            left.Effects.Add(previewEffect);

            //firstSteps.Add(up);
            firstSteps.Add(down);
            //firstSteps.Add(right);
            //firstSteps.Add(left);

            return firstSteps;
        }
    }
}
