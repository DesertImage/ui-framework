using UnityEngine;

namespace DesertImage.UI
{
    public class HorizontalUILayout : UILayoutBase
    {
        protected override Vector2 GetPosition(RectTransform currentRect, RectTransform previousRect)
        {
            var contentRect = content as RectTransform;

            if (!previousRect)
            {
                return new Vector2(contentRect.rect.xMin, contentRect.rect.yMin + currentRect.sizeDelta.x);
            }

            var previousPos = previousRect.anchoredPosition;
            var previousSize = previousRect.sizeDelta;

            return new Vector2(previousPos.x + previousSize.x + spacing, 0f);
        }
    }
}