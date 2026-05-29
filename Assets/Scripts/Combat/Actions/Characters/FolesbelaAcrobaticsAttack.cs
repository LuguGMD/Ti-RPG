using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using RPG.Extensions;
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
        [SerializeField] private Vector2Int _dir;

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
            PreviewTileInfo side;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            side = new PreviewTileInfo(_dir + _dir, _dir.ToDirection(), false);
            side.Effects.Add(_effects[0]);
            side.Effects.Add(_effects[1]);
            PreviewTileInfo child = side.CreateChild(_dir + _dir, _dir.ToDirection(), false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(_dir + _dir, _dir.ToDirection(), false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(_dir + _dir, _dir.ToDirection(), false);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);

            firstSteps.Add(side);

            return firstSteps;
        }
    }
}
