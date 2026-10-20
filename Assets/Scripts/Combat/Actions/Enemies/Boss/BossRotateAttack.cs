using RPG.Combat.Grid;
using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class BossRotateAttack : BossCombatAction
    {
        private int[] _rotateAmounts = new int[3];
        private float _rotationDuration = 0.8f;

        public override void Init(StageEntityController user)
        {
            _user = user;
        }

        public override IEnumerator Execute(PreviewTileInfo selectedPreviewTile)
        {
            yield return new WaitForSeconds(1f);
            for(int i = 0; i < _rotateAmounts.Length; i++)
            {
                int side = Random.Range(0, 2) == 0 ? -1 : 1;
                int rotateAmount = side * Random.Range(1, 5);
                SetRowRotateAmount(i, rotateAmount);

                MapManager.Instance.RotateRow(i, _rotateAmounts[i]);
                yield return new WaitForSeconds(_rotationDuration);
            }

            _lastTurnUsed = CombatManager.TurnCount;
        }

        public override List<PreviewTileInfo> Preview()
        {
            List<PreviewTileInfo> previewTilesInfo = new List<PreviewTileInfo>();
            return previewTilesInfo;
        }

        public void SetRowRotateAmount(int rowIndex, int rotateAmount)
        {
            if (rowIndex >= _rotateAmounts.Length) return;

            while(_rotateAmounts.Contains(rotateAmount) || rotateAmount == 0)
            {
                rotateAmount = Random.Range(-6, 6);
            }

            _rotateAmounts[rowIndex] = rotateAmount;
        }
    }
}
