using TMPro;
using UnityEngine;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class EnemyTooltip : Tooltip
    {
        [Header("Enemy Tooltip")]
        public TextMeshProUGUI combatField;

        public void SetEnemy(EnemyScriptable enemy)
        {
            if (enemy == null)
            {
                Debug.LogWarning("EnemyTooltip: EnemyScriptable está vazio!");
                return;
            }

            // Mostra o nome do inimigo.
            headerField.gameObject.SetActive(true);
            headerField.text = enemy.EntityName;

            // Mostra a descrição do inimigo.
            contentField.text = enemy.SpotlightDescription;

            // Verifica se a fase desse inimigo já foi concluída.
            bool combatUnlocked = false;

            if (enemy.Level != null)
            {
                combatUnlocked =
                    GameManager.CompletedLevels.Contains(enemy.Level.LevelKey);
            }

            // Mostra a descrição de combate somente se a fase estiver completa.
            if (combatField != null)
            {
                combatField.gameObject.SetActive(combatUnlocked);

                if (combatUnlocked)
                {
                    combatField.text = enemy.CombatDescription;
                }
            }
        }
    }
}