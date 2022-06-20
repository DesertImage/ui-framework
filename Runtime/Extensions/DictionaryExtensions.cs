using System.Collections.Generic;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out _))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}