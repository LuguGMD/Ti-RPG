using RPG.Combat.Actions.Effects;
using RPG.Combat.Preview;
using RPG.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using RPG.Audio;

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

            yield return new WaitForSeconds(0.2f / CombatManager.CombatSpeed);

            do
            {
                AudioManager.Instance.PlayOneShot(_accordionOpenSound);
                yield return _user.Movement.Move(new Movement(root.Direction, root.NeedsToBeEmpty), (int)root.RelativePosition.magnitude);
                AudioManager.Instance.PlayOneShot(_accordionClosedSound);
                AudioManager.Instance.PlayOneShot(_impactSound);
                foreach (Effect effect in _effects)
                {
                    effect.Execute(_user);
                }
                if (root == selectedPreviewTile) break;
                root = root.Child;

                yield return new WaitForSeconds(0.3f / CombatManager.CombatSpeed);
            } while (root != null);
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
            child = child.CreateChild(_dir + _dir, _dir.ToDirection(), false,true, false, true);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);
            child = child.CreateChild(_dir + _dir, _dir.ToDirection(), false,true,false, true);
            child.Effects.Add(_effects[0]);
            child.Effects.Add(_effects[1]);

            firstSteps.Add(side);

            return firstSteps;
        }
    }
}
