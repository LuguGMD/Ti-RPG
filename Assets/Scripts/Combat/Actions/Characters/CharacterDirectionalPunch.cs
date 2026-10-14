using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class CharacterDirectionalPunch : CombatAction
    {
        [SerializeField] private float _damage = 2;
        [SerializeField] private int _pushAmount = 1;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[0].Commands.Add(new PushEffect(Grid.DirectionEnum.Up, _pushAmount));

            if(_effects.Count > 1)
            {
                _effects[1].Commands.Add(new DamageEffect(_damage));
            }
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            do
            {
                _user.TileObject.SetDirection(root.Direction);
                foreach(Effect effect in _effects)
                {
                    effect.Execute(_user);
                }
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
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
            previewEffect.Area[0] = Vector2Int.zero;

            up = new PreviewTileInfo(Vector2Int.up, Grid.DirectionEnum.Up, false);
            up.Effects.Add(previewEffect);

            down = new PreviewTileInfo(Vector2Int.down, Grid.DirectionEnum.Down, false);
            down.Effects.Add(previewEffect);


            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, false);
            right.Effects.Add(previewEffect);


            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, false);
            left.Effects.Add(previewEffect);

            firstSteps.Add(up);
            firstSteps.Add(down);
            firstSteps.Add(right);
            firstSteps.Add(left);

            return firstSteps;
        }
    }
}
