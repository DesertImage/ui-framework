using UnityEngine;

namespace DesertImage.UI
{
    public class LayoutContentAutoSize : LayoutExtension
    {
        [SerializeField] private bool width = true;
        [SerializeField] private bool height;

        protected override void OnLayoutPreUpdate()
        {
            var contentRect = layout.Content as RectTransform;

            if (!contentRect) return;

            var childCount = contentRect.childCount;

            if (childCount == 0) return;

            if (width)
            {
                UpdateWidth(contentRect);
            }

            if (height)
            {
                UpdateHeight(contentRect);
            }
        }

        private void UpdateWidth(RectTransform content)
        {
            var childCount = content.childCount;

            var spacingSum = (childCount - 1) * layout.Spacing;

            var sum = content.GetChildWidthSum();

            content.sizeDelta = new Vector2(spacingSum + sum, content.sizeDelta.y);
        }

        private void UpdateHeight(RectTransform content)
        {
            var childCount = content.childCount;

            var spacingSum = (childCount - 1) * layout.Spacing;

            var sum = content.GetChildHeightSum();

            content.sizeDelta = new Vector2(content.sizeDelta.x, spacingSum + sum);
        }
    }
}
