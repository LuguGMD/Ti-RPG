using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class DonLiponWeightAttack : CombatAction
    {
        [SerializeField] private float _damage;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[1].Commands.Add(new DamageEffect(_damage));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            do
            {
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);

            yield return new WaitForSeconds(1.15f / CombatManager.CombatSpeed);
            foreach (Effect effect in _effects)
            {
                effect.Execute(_user);
            }

        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo up;
            PreviewTileInfo down;
            PreviewTileInfo right;
            PreviewTileInfo left;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            up = new PreviewTileInfo(Vector2Int.up, Grid.DirectionEnum.Up, true, false);
            up.Effects.Add(_effects[0]);
            PreviewTileInfo child = up.CreateChild(Vector2Int.up, Grid.DirectionEnum.Up, true, false);
            child.Effects.Add(_effects[0]);

            down = new PreviewTileInfo(Vector2Int.down, Grid.DirectionEnum.Down, true, false);
            down.Effects.Add(_effects[0]);
            child = down.CreateChild(Vector2Int.down, Grid.DirectionEnum.Down, true, false);
            child.Effects.Add(_effects[0]);


            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, true, false);
            right.Effects.Add(_effects[0]);
            child = right.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, true, false);
            child.Effects.Add(_effects[0]);


            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, true, false);
            left.Effects.Add(_effects[0]);
            child = left.CreateChild(Vector2Int.left, Grid.DirectionEnum.Left, true, false);
            child.Effects.Add(_effects[0]);

            firstSteps.Add(up);
            firstSteps.Add(down);
            firstSteps.Add(right);
            firstSteps.Add(left);

            return firstSteps;
        }
    }
}
