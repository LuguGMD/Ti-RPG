using FMODUnity;
using RPG.Audio;
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
    public class FolesbelaAcrobaticsAttack : CombatAction
    {
        [SerializeField] private float _damage;
        [SerializeField] private Vector2Int _dir;
        [SerializeField] private EventReference _accordionOpenSound;
        [SerializeField] private EventReference _accordionClosedSound;
        [SerializeField] private EventReference _impactSound;

        public override void Init(StageEntityController user)
        {
            _user = user;
            _effects[0].Commands.Add(new DamageEffect(_damage));
            _effects[1].Commands.Add(new DamageEffect(_damage));
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            PreviewTileInfo root = PreviewTileInfo.GetRoot(selectedPreviewTile);
            bool didHit = false;

            Vector2Int targetTile = _user.Position + selectedPreviewTile.RelativePosition;
            Vector3 targetPos = MapManager.Instance.GetWorldPosition(targetTile);
            targetPos.y = _user.transform.position.y;

            _user.transform.LookAt(targetPos);
            _user.Model.transform.localEulerAngles = Vector3.up * 180f;

            int repetitions = 0;

            do
            {
                if (repetitions % 2 == 0)
                    yield return new WaitForSeconds(0.58f / CombatManager.CombatSpeed);
                AudioManager.Instance.PlayOneShot(_accordionOpenSound);
                float waitTime = repetitions % 2 == 0 ? 0.3f : 0.26f;
                yield return _user.Movement.Move(new Movement(root.Direction, root.NeedsToBeEmpty), (int)root.RelativePosition.magnitude, waitTime);
                AudioManager.Instance.PlayOneShot(_accordionClosedSound);
                AudioManager.Instance.PlayOneShot(_impactSound);
                foreach (Effect effect in _effects)
                {
                    if (effect.Execute(_user))
                    {
                        didHit = true;
                    }
                }
                if (root == selectedPreviewTile) break;
                root = root.Child;
                waitTime = repetitions % 2 == 0 ? 0.12f : 0.25f;
                yield return new WaitForSeconds(waitTime / CombatManager.CombatSpeed);
                repetitions++;
            } while (root != null);

            _user.SetAnimationBool("IsActionRunning", false);
            _user.Model.transform.localEulerAngles = Vector3.zero;

            if (didHit)
            {
                CombatManager.Instance.HealCharge(_user, (_user.Info as CharacterScriptable).HealAmount);
            }
        }

        public override List<PreviewTileInfo> Preview()
        {
            PreviewTileInfo side;

            List<PreviewTileInfo> firstSteps = new List<PreviewTileInfo>();

            side = new PreviewTileInfo(_dir + _dir, _dir.ToDirection(), false, true, false, true);
            side.Effects.Add(_effects[0]);
            side.Effects.Add(_effects[1]);
            PreviewTileInfo child = side.CreateChild(_dir + _dir, _dir.ToDirection(), false, true, false, true);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(_dir + _dir, _dir.ToDirection(), false, true, false, true);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(_dir + _dir, _dir.ToDirection(), false, true, false, true);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);

            firstSteps.Add(side);

            return firstSteps;
        }
    }
}
