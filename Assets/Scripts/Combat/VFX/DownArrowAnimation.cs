using UnityEngine;
using DG.Tweening;

namespace RPG
{
    public class DownArrowAnimation : MonoBehaviour
    {
        [SerializeField] float verticalDisplacement = 0.2f;
        [SerializeField] float duration = 0.5f;

        void Start()
        {
            transform.DOLocalMoveY(transform.localPosition.y + verticalDisplacement, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
    }
}
