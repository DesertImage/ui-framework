using UnityEngine;

namespace DesertImage.UI
{
    public class UILayoutBase : LayoutBase
    {
        protected override void UpdatePosition(int index)
        {
            base.UpdatePosition(index);

            var currentRect = content.GetChild(index) as RectTransform;

            currentRect.anchorMin = new Vector2(.5f, .5f);
            currentRect.anchorMax = new Vector2(.5f, .5f);

            var previousRect = index > 0 ? content.GetChild(index - 1) as RectTransform : null;

            currentRect.anchoredPosition = GetPosition(currentRect, previousRect);

            OnUpdate();
        }

        protected virtual Vector2 GetPosition(RectTransform currentRect, RectTransform previousRect)
        {
            return Vector2.zero;
        }
    }
}