using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class BarbaraSongAttack : CombatAction
    {
        [SerializeField] private float _heal;
        [SerializeField] private float _damage;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new HealingEffect(_heal));
            _effects[1].Commands.Add(new DamageEffect(_damage));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            do
            {
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
                foreach (Effect effect in root.Effects)
                {
                    effect.Execute(_user);
                }
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

            PreviewTileInfo upRight;
            PreviewTileInfo upLeft;

            PreviewTileInfo downRight;
            PreviewTileInfo downLeft;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            up = new PreviewTileInfo(Vector2Int.up, Grid.DirectionEnum.Up, true);
            up.Effects.Add(_effects[0]);
            up.Effects.Add(_effects[1]);

            down = new PreviewTileInfo(Vector2Int.down, Grid.DirectionEnum.Down, true);
            down.Effects.Add(_effects[0]);
            down.Effects.Add(_effects[1]);

            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, true);
            right.Effects.Add(_effects[0]);
            right.Effects.Add(_effects[1]);

            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, true);
            left.Effects.Add(_effects[0]);
            left.Effects.Add(_effects[1]);

            upRight = new PreviewTileInfo(Vector2Int.up + Vector2Int.right, Grid.DirectionEnum.UpRight, true);
            upRight.Effects.Add(_effects[0]);
            upRight.Effects.Add(_effects[1]);

            upLeft = new PreviewTileInfo(Vector2Int.up - Vector2Int.right, Grid.DirectionEnum.UpLeft, true);
            upLeft.Effects.Add(_effects[0]);
            upLeft.Effects.Add(_effects[1]);

            downRight = new PreviewTileInfo(Vector2Int.down + Vector2Int.right, Grid.DirectionEnum.DownRight, true);
            downRight.Effects.Add(_effects[0]);
            downRight.Effects.Add(_effects[1]);

            downLeft = new PreviewTileInfo(Vector2Int.down - Vector2Int.right, Grid.DirectionEnum.DownLeft, true);
            downLeft.Effects.Add(_effects[0]);
            downLeft.Effects.Add(_effects[1]);

            firstSteps.Add(up);
            firstSteps.Add(down);
            firstSteps.Add(right);
            firstSteps.Add(left);

            firstSteps.Add(upRight);
            firstSteps.Add(upLeft);
            firstSteps.Add(downRight);
            firstSteps.Add(downLeft);

            return firstSteps;
        }
    }
}
