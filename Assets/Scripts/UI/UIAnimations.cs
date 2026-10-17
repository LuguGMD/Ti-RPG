using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace RPG.UI
{
    public static class UIAnimations
    {
        public static IEnumerator RevealRectTransformScale(RectTransform rect, float shakeDuration = 1f, float scaleDuration = 1f, float initialScale = 0.1f)
        {
            Vector2 anchorPos = rect.anchoredPosition;
            rect.localScale = Vector3.one * initialScale;
            rect.DOShakeAnchorPos(shakeDuration, 10f).OnComplete(() =>
            {
                rect.anchoredPosition = anchorPos;
                rect.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutElastic);
            });

            yield return new WaitForSeconds((shakeDuration + scaleDuration) + 0.5f);
        }
    }
}
