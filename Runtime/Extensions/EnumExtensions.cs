using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace DesertImage.Extensions
{
    public static class EnumExtensions
    {
        public static T GetRandomElement<T>(this T enumCollection) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static T GetRandomValue<T>(this object sender) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static T GetRandomValue<T>(this object sender, T exceptItem) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));

            var value = (T)values.GetValue(Random.Range(0, values.Length));

            if (Equals(value, exceptItem))
            {
                value = GetRandomValue(null, exceptItem);
            }

            return value;
        }

        public static T GetRandomValue<T>(this object sender, T[] exceptItems) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));

            var value = (T)values.GetValue(Random.Range(0, values.Length));

            return exceptItems.Contains(value) ? GetRandomValue(null, exceptItems) : value;
        }

        public static int GetLength<T>(this object sender) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public static T GetRandomValue<T>(this object sender, ushort minIndex, ushort maxIndex, T exceptItem = default)
            where T : Enum
        {
            var values = Enum.GetValues(typeof(T));

            var value = (T)values.GetValue(Random.Range(minIndex, maxIndex + 1));

            if (value.CompareTo(exceptItem) == 0)
            {
                value = GetRandomValue(null, minIndex, maxIndex, exceptItem);
            }

            return value;
        }

        public static T GetValueByName<T>(this object sender, string name = "") where T : Enum
        {
            var ids = (T[])Enum.GetValues(typeof(T));

            return (from objId in ids where objId.ToString().ToLower().Contains(name.ToLower()) select objId)
                .FirstOrDefault();
        }
    }
}