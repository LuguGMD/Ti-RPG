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
            if (enemy == null)
            {
                Debug.LogWarning("EnemyTooltip: EnemyScriptable está vazio!");
                return;
            }

            // Nome do inimigo.
            headerField.gameObject.SetActive(true);
            headerField.text = enemy.EntityName;

            // Descrição do personagem.
            contentField.text = enemy.SpotlightDescription;

            // Descrição do combate.
            if (combatField != null)
            {
                combatField.gameObject.SetActive(_combatUnlocked);

                if (_combatUnlocked)
                {
                    combatField.text = enemy.CombatDescription;
                }
            }
        }

        public void UnlockCombatDescription()
        {
            _combatUnlocked = true;
        }
    }
}