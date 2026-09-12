using TMPro;
using UnityEngine;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class EnemyTooltip : Tooltip
    {
        [Header("Enemy Tooltip")]
        public TextMeshProUGUI combatField;

        private bool _combatUnlocked;

        public void SetEnemy(EnemyScriptable enemy)
        {
            headerField.gameObject.SetActive(true);
            headerField.text = enemy.EntityName;

            contentField.text = enemy.SpotlightDescription;

            combatField.gameObject.SetActive(_combatUnlocked);

            if (_combatUnlocked)
            {
                combatField.text = enemy.CombatDescription;
            }
        }

        public void UnlockCombatDescription()
        {
            _combatUnlocked = true;
        }
    }
}