using System;

namespace DesertImage.Extensions
{
    public static class MathExtensions
    {
        public static int RoundTo10(this int i, int nearest = 10)
        {
            if (nearest <= 0 || nearest % 10 != 0)
                throw new ArgumentOutOfRangeException(nameof(nearest), "Must round to a positive multiple of 10");

            return (i + 5 * nearest / 10) / nearest * nearest;
        }
    }
}