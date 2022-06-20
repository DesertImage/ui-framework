using UnityEngine;

namespace DesertImage.Extensions
{
    public static partial class CanvasExtensions
    {
        public static float GetChildWidthSum(this Transform transform)
        {
            var sum = 0f;

            if (transform.childCount == 0) return 0f;

            for (var i = 0; i < transform.childCount; i++)
            {
                var childRect = transform.GetChild(i) as RectTransform;

                sum += childRect.sizeDelta.x;
            }

            return sum;
        }

        public static float GetChildHeightSum(this Transform transform)
        {
            var sum = 0f;

            if (transform.childCount == 0) return 0f;

            for (var i = 0; i < transform.childCount; i++)
            {
                var childRect = transform.GetChild(i) as RectTransform;

                sum += childRect.sizeDelta.y;
            }

            return sum;
        }
    }
}