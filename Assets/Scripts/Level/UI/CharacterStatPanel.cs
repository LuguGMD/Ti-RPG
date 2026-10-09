using RPG.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Level.UI
{
    public class CharacterStatPanel : MonoBehaviour
    {
        public enum StatType
        {
            Motivation,
            Strength,
            Mobility
        }

        [SerializeField] private StatType _statType;
        [SerializeField] private List<Image> _statValueImages;

        public void UpdateInfo(CharacterScriptable character)
        {
            int statValue = 0;
            switch (_statType)
            {
                case StatType.Motivation:
                    statValue = character.MotivationStat;
                    break;
                case StatType.Strength:
                    statValue = character.StrengthStat;
                    break;
                case StatType.Mobility:
                    statValue = character.MobilityStat;
                    break;
            }
            for (int i = 0; i < _statValueImages.Count; i++)
            {
                _statValueImages[i].enabled = i < statValue;
            }
        }
    }
}
