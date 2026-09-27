using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    public class BossRotateAttack : CombatAction
    {
        private int[] _rotateAmounts = new int[3];
        private float _rotationDuration = 3f;

        public override void Init(StageEntityController user)
        {
            
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            yield return new WaitForSeconds(1f);
            for(int i = 0; i < _rotateAmounts.Length; i++)
            {
                MapManager.Instance.RotateRow(i, _rotateAmounts[i]);
                yield return new WaitForSeconds(_rotationDuration);
            }
        }

        public override List<PreviewTileInfo> Preview()
        {
            List<PreviewTileInfo> previewTilesInfo = new List<PreviewTileInfo>();
            return previewTilesInfo;
        }

        public void SetRowRotateAmount(int rowIndex, int rotateAmount)
        {
            if (rowIndex >= _rotateAmounts.Length) return;

            _rotateAmounts[rowIndex] = rotateAmount;
        }
    }
}
