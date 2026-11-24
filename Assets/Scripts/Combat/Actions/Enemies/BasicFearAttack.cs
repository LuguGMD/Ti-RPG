using FMODUnity;
using RPG.Audio;
using RPG.Combat.Actions.Effects;
using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class BasicFearAttack : CombatAction
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

            AudioManager.Instance.PlayOneShot(_attackSFX);

            Vector2Int targetTile = _user.Position + selectedPreviewTile.RelativePosition;
            Vector3 targetPos = MapManager.Instance.GetWorldPosition(targetTile);
            targetPos.y = _user.transform.position.y;

            _user.transform.LookAt(targetPos);

            do
            {
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
            PreviewTileInfo downLeft;
            PreviewTileInfo downright;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            Effect previewEffect = Effect.Clone(_effects[0]);
            previewEffect.Area[0] = new Vector2Int(0, 0);

            downLeft = new PreviewTileInfo(Vector2Int.down + Vector2Int.left, Grid.DirectionEnum.DownLeft, true, alwaysShowEffect: true);
            downLeft.Effects.Add(previewEffect);

            downright = new PreviewTileInfo(Vector2Int.down + Vector2Int.right, Grid.DirectionEnum.DownRight, true, alwaysShowEffect: true);
            downright.Effects.Add(previewEffect);

            if (CombatManager.TurnCount % 2 == 0)
            {
                firstSteps.Add(downLeft);
                _effects[0].Area[0] = new Vector2Int(-1, -1);
                _effects[1].Area[0] = new Vector2Int(-1, -1);
            }
            if (CombatManager.TurnCount % 2 == 1)
            {
                firstSteps.Add(downright);
                _effects[0].Area[0] = new Vector2Int(1, -1);
                _effects[1].Area[0] = new Vector2Int(1, -1);
            }

            return firstSteps;
        }
    }
}
