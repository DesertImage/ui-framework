using UnityEngine;

namespace DesertImage.UI
{
    public class VerticalUILayout : UILayoutBase
    {
        protected override Vector2 GetPosition(RectTransform currentRect, RectTransform previousRect)
        {
            var contentRect = content as RectTransform;

            if (!previousRect) return new Vector2(0f, contentRect.rect.yMax - currentRect.sizeDelta.y * .5f);

            var previousPos = previousRect.anchoredPosition;
            var previousSize = previousRect.sizeDelta;

            return new Vector2(0f, previousPos.y - (previousSize.y + spacing));
        }
    }
}