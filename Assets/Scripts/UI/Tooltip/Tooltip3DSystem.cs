using UnityEngine;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class Tooltip3DSystem : MonoBehaviour
    {
        [Header("Tooltip 3D")]
        [SerializeField] private EnemyTooltip enemyTooltip;

        private EnemyTooltip GetEnemyTooltip()
        {
            if (enemyTooltip == null)
            {
                enemyTooltip = FindFirstObjectByType<EnemyTooltip>(
                    FindObjectsInactive.Include
                );
            }

            return enemyTooltip;
        }

        public void ShowEnemy(
            EnemyScriptable enemy,
            Vector3 worldPosition,
            Vector2 pivot
        )
        {
            if (enemy == null)
                return;

            EnemyTooltip tooltip = GetEnemyTooltip();

            if (tooltip == null)
            {
                Debug.LogError(
                    "Tooltip3DSystem: EnemyTooltip não encontrado!"
                );

                return;
            }

            Debug.Log("Tooltip3DSystem: MOSTRANDO TOOLTIP!");

            tooltip.SetEnemy(enemy);

            tooltip.gameObject.SetActive(true);

            // Converte a posição 3D do inimigo para posição na tela.
            Vector3 screenPosition =
                UnityEngine.Camera.main.WorldToScreenPoint(worldPosition);

            tooltip.rectTransform.position = screenPosition;
            tooltip.rectTransform.pivot = pivot;

            tooltip.Show();
        }

        public void Hide()
        {
            EnemyTooltip tooltip = GetEnemyTooltip();

            if (tooltip == null)
            {
                Debug.LogError(
                    "Tooltip3DSystem: Não encontrou EnemyTooltip para esconder!"
                );

                return;
            }

            Debug.Log("Tooltip3DSystem: ESCONDENDO TOOLTIP!");

            tooltip.Hide();
        }
    }
}