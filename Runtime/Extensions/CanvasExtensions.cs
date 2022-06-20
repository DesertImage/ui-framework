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
        
        public static void SetStretch(this RectTransform rect)
        {
            rect.transform.localScale = Vector3.one;

            rect.anchoredPosition = new Vector2();

            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 1f);

            rect.pivot = new Vector2(0.5f, 0.5f);

            rect.offsetMin = rect.offsetMax = new Vector2(0, 0);
        }
    }
}
