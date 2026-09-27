using RPG.Combat.Preview;
using RPG.Combat.Wave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class EnemySpawnAction : CombatAction
    {
        private EnemySpawnInfo _spawnInfo;
        private bool _shouldApplySpotlightEffect = false;

        #region Properties

        public EnemySpawnInfo SpawnInfo { get { return _spawnInfo; } }
        public bool ShouldApplySpotlightEffect { get { return _shouldApplySpotlightEffect; } }

        #endregion

        public EnemySpawnAction(EnemySpawnInfo spawnInfo, bool shouldApplySpotlightEffect = false)
        {
            _spawnInfo = spawnInfo;
            _shouldApplySpotlightEffect = shouldApplySpotlightEffect;
        }

        public override void Init(StageEntityController user)
        {

        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            EnemyController enemyInstance = CombatFactory.InstantiateEnemy(_spawnInfo);
            
            yield return new WaitForSeconds(0.1f / CombatManager.CombatSpeed);
        }

        public override List<PreviewTileInfo> Preview()
        {
            return new List<PreviewTileInfo>();
        }
    }
}
