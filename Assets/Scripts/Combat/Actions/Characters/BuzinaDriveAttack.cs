using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using RPG.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class BuzinaDriveAttack : CombatAction
    {
        [SerializeField] private float _baseDamage = 2;
        [SerializeField] private float _movementDamage = 2;
        [SerializeField] private int _pushAmount = 1;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new PushEffect(Grid.DirectionEnum.Up, _pushAmount));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            Vector2Int startPos = _user.Position;

            Vector2Int targetTile = _user.Position + root.Direction.ToVector2Int();
            Vector3 targetPos = MapManager.Instance.GetWorldPosition(targetTile);
            _user.transform.LookAt(targetPos);

            yield return new WaitForSeconds(1.09f / CombatManager.CombatSpeed);

            do
            {
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
                
                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);

            int movedDistance = Mathf.Abs(startPos.x - _user.Position.x);
            _effects[0].Commands.Add(new DamageEffect(_baseDamage + (_movementDamage * movedDistance)));
            _effects[1].Commands.Add(new DamageEffect(_baseDamage + (_movementDamage * movedDistance)));

            foreach (Effect effect in _effects)
            {
                effect.Execute(_user);
            }

            _effects[0].Commands.RemoveAt(_effects[0].Commands.Count-1);
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo right;
            PreviewTileInfo left;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, true);
            right.Effects.Add(_effects[0]);
            PreviewTileInfo child = right.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, true);
            child.Effects.Add(_effects[0]);
            child = child.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, true);
            child.Effects.Add(_effects[0]);
            child = child.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, true);
            child.Effects.Add(_effects[0]);


            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, true);
            left.Effects.Add(_effects[0]);
            child = left.CreateChild(Vector2Int.left, Grid.DirectionEnum.Left, true);
            child.Effects.Add(_effects[0]);
            child = child.CreateChild(Vector2Int.left, Grid.DirectionEnum.Left, true);
            child.Effects.Add(_effects[0]);
            child = child.CreateChild(Vector2Int.left, Grid.DirectionEnum.Left, true);
            child.Effects.Add(_effects[0]);

            firstSteps.Add(right);
            firstSteps.Add(left);

            return firstSteps;
        }
    }
}
