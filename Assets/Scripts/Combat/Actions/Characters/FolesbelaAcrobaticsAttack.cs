using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class FolesbelaAcrobaticsAttack : CombatAction
    {
        [SerializeField] private float _damage;
        [SerializeField] private int _pushAmount;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[1].Commands.Add(new PushEffect(Grid.DirectionEnum.Up, _pushAmount, false));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            do
            {
                yield return _user.Movement.Move(new Movement(root.Direction, root.NeedsToBeEmpty), (int)root.RelativePosition.magnitude);
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
            PreviewTileInfo right;
            PreviewTileInfo left;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, false);
            right.Effects.Add(_effects[0]);
            right.Effects.Add(_effects[1]);
            PreviewTileInfo child = right.CreateChild(Vector2Int.right + Vector2Int.right, Grid.DirectionEnum.Right, false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(Vector2Int.right + Vector2Int.right, Grid.DirectionEnum.Right, false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);


            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, false);
            left.Effects.Add(_effects[0]);
            left.Effects.Add(_effects[1]);
            child = left.CreateChild(Vector2Int.left + Vector2Int.left, Grid.DirectionEnum.Left, false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(Vector2Int.left + Vector2Int.left, Grid.DirectionEnum.Left, false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);

            firstSteps.Add(right);
            firstSteps.Add(left);

            return firstSteps;
        }
    }
}
