using FMODUnity;
using RPG.Audio;
using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using RPG.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class MediumSadnessScreamAttack : CombatAction
    {
        private const float shakeCameraForce = 0.3f;

        [SerializeField] private float _damage;
        [SerializeField] private EventReference _attackSFX;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[0].Commands.Add(new PushRelativeEffect(1));
            _effects[1].Commands.Add(new DamageEffect(_damage));
            _effects[2].Commands.Add(new PushRelativeEffect(1));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            AudioManager.Instance.PlayOneShot(_attackSFX);
            yield return new WaitForSeconds(0.3f / CombatManager.CombatSpeed);

            CombatManager.Instance.CameraShake(shakeCameraForce);

            foreach (Effect effect in _effects)
            {
                effect.Execute(_user);
            }

            yield return new WaitForSeconds(0.3f / CombatManager.CombatSpeed);
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo up;
            PreviewTileInfo upLeft;
            PreviewTileInfo left;
            PreviewTileInfo downLeft;
            PreviewTileInfo down;
            PreviewTileInfo downRight;
            PreviewTileInfo right;
            PreviewTileInfo upRight;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();
            Effect previewEffect = Effect.Clone(_effects[0]);
            previewEffect.Area.Clear();
            previewEffect.Area.Add(Vector2Int.zero);

            up = new PreviewTileInfo(Vector2Int.up, Vector2Int.up.ToDirection(), false, alwaysShowEffect: true);
            up.Effects.Add(previewEffect);
            upLeft = new PreviewTileInfo(Vector2Int.up + Vector2Int.left, Vector2Int.left.ToDirection(), false, alwaysShowEffect: true);
            upLeft.Effects.Add(previewEffect);
            left = new PreviewTileInfo(Vector2Int.left, Vector2Int.left.ToDirection(), false, alwaysShowEffect: true);
            left.Effects.Add(previewEffect);
            downLeft = new PreviewTileInfo(Vector2Int.down + Vector2Int.left, Vector2Int.left.ToDirection(), false, alwaysShowEffect: true);
            downLeft.Effects.Add(previewEffect);
            down = new PreviewTileInfo(Vector2Int.down, Vector2Int.down.ToDirection(), false, alwaysShowEffect: true);
            down.Effects.Add(previewEffect);
            downRight = new PreviewTileInfo(Vector2Int.down + Vector2Int.right, Vector2Int.right.ToDirection(), false, alwaysShowEffect: true);
            downRight.Effects.Add(previewEffect);
            right = new PreviewTileInfo(Vector2Int.right, Vector2Int.right.ToDirection(), false, alwaysShowEffect: true);
            right.Effects.Add(previewEffect);
            upRight = new PreviewTileInfo(Vector2Int.up + Vector2Int.right, Vector2Int.right.ToDirection(), false, alwaysShowEffect: true);
            upRight.Effects.Add(previewEffect);


            firstSteps.Add(up);
            firstSteps.Add(upLeft);
            firstSteps.Add(left);
            firstSteps.Add(downLeft);
            firstSteps.Add(down);
            firstSteps.Add(downRight);
            firstSteps.Add(right);
            firstSteps.Add(upRight);

            return firstSteps;
        }
    }
}
