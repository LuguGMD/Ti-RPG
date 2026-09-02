using UnityEngine;

namespace RPG.Combat
{
    public class BossController : EnemyController
    {
        // O boss é uma extensão do enemy controller para se aproveitar do gerenciador de combate e do sistema de ações
        // Sendo o primeiro inimigo, sempre age antes dos outros

        public override void PrepareAction()
        {
            // logica para escolher ação via SelectAction( indice );
            // as ações precisam existir no _actions, ou serem hardcodadas
        }
    }
}
