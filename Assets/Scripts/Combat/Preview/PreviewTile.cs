using RPG.Combat.Grid;
using RPG.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Preview
{
    public class PreviewTile : MonoBehaviour
    {
        public void SetPosition(Vector2Int tilePosition)
        {
            tilePosition = tilePosition.ClampMap();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i)?.gameObject.SetActive(i == tilePosition.y);
            }

            transform.position = MapManager.Instance.GetWorldPostion(tilePosition);
            transform.LookAt(transform.position - (transform.position.normalized));
        }
    }
}
