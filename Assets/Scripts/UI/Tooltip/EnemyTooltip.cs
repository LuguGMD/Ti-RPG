using TMPro;
using UnityEngine;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class EnemyTooltip : Tooltip
    {
        [Header("Enemy Tooltip")]
        public TextMeshProUGUI combatField;

        public void SetEnemy(EnemyScriptable enemy, bool combatUnlocked)
        {
            // Mostra o nome do inimigo.
            headerField.gameObject.SetActive(true);
            headerField.text = enemy.EntityName;

            // Mostra a descrição do personagem.
            contentField.text = enemy.SpotlightDescription;

            // Mostra ou esconde a descrição de combate.
            combatField.gameObject.SetActive(combatUnlocked);

            if (combatUnlocked)
            {
                combatField.text = enemy.CombatDescription;
            }
        }
    }
}