using UnityEngine;

namespace Extensions
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha = 1f)
        {
            color.a = alpha;

            return color;
        }

        public static Color[] ConvertToColors(this string[] hexArray, Color[] cachedArray = null)
        {
            if ((hexArray?.Length ?? 0) == 0) return cachedArray;

            var array = cachedArray ?? new Color[hexArray.Length];

            for (var i = 0; i < hexArray.Length; i++)
            {
                ColorUtility.TryParseHtmlString("#" + hexArray[i], out var color);

                array[i] = color;
            }

            return array;
        }
    }
}