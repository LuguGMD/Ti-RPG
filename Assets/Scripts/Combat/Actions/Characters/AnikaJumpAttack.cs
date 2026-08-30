using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class AnikaJumpAttack : CombatAction
    {
        [SerializeField] private float _damage;
        [SerializeField] private int _pushAmount;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[0].Commands.Add(new PushRelativeEffect(_pushAmount));

            _effects[1].Commands.Add(new DamageEffect(_damage));
        }


        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);

            do
            {
                _user.Movement.Teleport(Grid.DirectionEnum.Up, _user.Position + root.RelativePosition);
                Debug.Log(_user.Position + root.RelativePosition);
                yield return new WaitForSeconds(4.5f / CombatManager.CombatSpeed);
                _user.TileObject.UpdatePosition();
                foreach (Effect effect in _effects)
                {
                    effect.Execute(_user);
                }
                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);

        }


        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo firstLane;
            PreviewTileInfo secondLane;
            PreviewTileInfo thirdLane;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            Vector2Int position = new Vector2Int(6, 0 - _user.Position.y);
            firstLane = new PreviewTileInfo(position, Grid.DirectionEnum.Up, true);
            firstLane.Effects.Add(_effects[0]);

            position.y = 1 - _user.Position.y;
            secondLane = new PreviewTileInfo(position, Grid.DirectionEnum.Up, true);
            secondLane.Effects.Add(_effects[0]);

            position.y = 2 - _user.Position.y;
            thirdLane = new PreviewTileInfo(position, Grid.DirectionEnum.Up, true);
            thirdLane.Effects.Add(_effects[0]);

            firstSteps.Add(firstLane);
            firstSteps.Add(secondLane);
            firstSteps.Add(thirdLane);

            return firstSteps;
        }
    }
}
