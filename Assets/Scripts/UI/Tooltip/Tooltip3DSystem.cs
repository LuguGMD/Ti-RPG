using UnityEngine;
using RPG.Combat;

namespace RPG.UI.Tooltip
{
    public class Tooltip3DSystem : MonoBehaviour
    {
        [SerializeField] private EnemyTooltip _enemyTooltip;

        private static Tooltip3DSystem _instance;

        private void Awake()
        {
            _instance = this;

            // Procura o EnemyTooltip mesmo que ele esteja desativado na cena.
            if (_enemyTooltip == null)
            {
                _enemyTooltip =
                    FindFirstObjectByType<EnemyTooltip>(
                        FindObjectsInactive.Include
                    );
            }

            if (_enemyTooltip == null)
            {
                Debug.LogWarning("EnemyTooltip não foi encontrado na cena!");
            }
            else
            {
                Debug.Log("EnemyTooltip encontrado!");
            }
        }

        public static void ShowEnemy(
            EnemyScriptable enemy,
            Vector3 worldPosition,
            Vector2 pivot
        )
        {
            Debug.Log("ShowEnemy do Tooltip3DSystem foi chamado!");

            if (_instance == null)
            {
                Debug.LogWarning("Tooltip3DSystem não foi encontrado na cena!");
                return;
            }

            if (_instance._enemyTooltip == null)
            {
                Debug.LogWarning("EnemyTooltip não foi configurado!");
                return;
            }

            // Converte a posição do inimigo 3D para uma posição na tela.
            Vector3 screenPosition =
                UnityEngine.Camera.main.WorldToScreenPoint(worldPosition);

            // Preenche as informações do inimigo.
            _instance._enemyTooltip.SetEnemy(enemy, true);

            // Mostra o painel.
            _instance._enemyTooltip.gameObject.SetActive(true);

            // Coloca o painel na posição do inimigo na tela.
            _instance._enemyTooltip.rectTransform.position = screenPosition;
            _instance._enemyTooltip.rectTransform.pivot = pivot;

            _instance._enemyTooltip.Show();
        }

        public static void Hide()
        {
            if (_instance == null)
                return;

            if (_instance._enemyTooltip == null)
                return;

            _instance._enemyTooltip.Hide();
        }
    }
}