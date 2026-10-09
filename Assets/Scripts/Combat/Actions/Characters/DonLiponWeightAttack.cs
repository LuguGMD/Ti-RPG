using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using RPG.Audio;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class DonLiponWeightAttack : CombatAction
    {
        private const float shakeCameraForce = 0.4f;

        [SerializeField] private float _damage;
        [SerializeField] private EventReference _weightWindSound;
        [SerializeField] private EventReference _weightImpactSound;

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
                AudioManager.Instance.PlayOneShot(_weightWindSound);
                yield return _user.Movement.Move(new Movement(root.Direction, true), 1);
                if (root == selectedPreviewTile) break;
                root = root.Child;
            } while (root != null);

            yield return new WaitForSeconds(1.15f / CombatManager.CombatSpeed);
            CombatManager.Instance.CameraShake(shakeCameraForce);
            bool didHit = false;
            foreach (Effect effect in _effects)
            {
                if(effect.Execute(_user))
                {
                    didHit = true;
                }
            }
            if(didHit)
            {
                AudioManager.Instance.PlayOneShot(_weightImpactSound);
            }
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

            up = new PreviewTileInfo(Vector2Int.up, Grid.DirectionEnum.Up, false, false);
            up.Effects.Add(previewEffect);
            /*PreviewTileInfo child = up.CreateChild(Vector2Int.up, Grid.DirectionEnum.Up, false, false);
            child.Effects.Add(previewEffect);*/

            down = new PreviewTileInfo(Vector2Int.down, Grid.DirectionEnum.Down, false, false);
            down.Effects.Add(previewEffect);
            /*child = down.CreateChild(Vector2Int.down, Grid.DirectionEnum.Down, false, false);
            child.Effects.Add(previewEffect);*/


            right = new PreviewTileInfo(Vector2Int.right, Grid.DirectionEnum.Right, false, false);
            right.Effects.Add(previewEffect);
            /*child = right.CreateChild(Vector2Int.right, Grid.DirectionEnum.Right, false, false);
            child.Effects.Add(previewEffect);*/ 


            left = new PreviewTileInfo(Vector2Int.left, Grid.DirectionEnum.Left, false, false);
            left.Effects.Add(previewEffect);
            /*child = left.CreateChild(Vector2Int.left, Grid.DirectionEnum.Left, false, false);
            child.Effects.Add(previewEffect);*/

            firstSteps.Add(up);
            firstSteps.Add(down);
            firstSteps.Add(right);
            firstSteps.Add(left);

            return firstSteps;
        }
    }
}
