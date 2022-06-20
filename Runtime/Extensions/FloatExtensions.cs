using UnityEngine;

namespace DesertImage.Extensions
{
    public static class FloatExtensions
    {
        public static float Jiggle(this float value, float koef = .7f)
        {
            return value + Random.Range(-koef, koef);
        }

        public static float Jiggle(this float value, float minKoef, float maxKoef)
        {
            return value + Random.Range(-Random.Range(minKoef, maxKoef), Random.Range(minKoef, maxKoef));
        }

        /// <summary>
        /// Checking for random success
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static bool IsSuccessProbability(this float probability)
        {
            var random = Random.Range(0f, 100f);

            return random <= probability;
        }

        public static float GetPercentage(this float value, float percentage)
        {
            return value * percentage * .01f;
        }
    }
}