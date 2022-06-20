using DesertImage.Extensions;

namespace External.Extensions
{
    public static class CustomDictionaryExtensions
    {
        public static TValue GetNextValue<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
            bool isLoop = false)
        {
            if (dictionary.Count == 0) return default;

            TValue DefaultValue()
            {
                return isLoop ? dictionary[0].Item2 : default;
            }

            var indexOf = dictionary.Keys.IndexOf(key);

            if (indexOf == -1) return isLoop ? dictionary[0].Item2 : DefaultValue();

            return indexOf >= dictionary.Count - 1 ? DefaultValue() : dictionary[indexOf + 1].Item2;
        }

        public static TValue GetPreviousValue<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
            bool isLoop = false)
        {
            if (dictionary.Count == 0) return default;

            TValue DefaultValue()
            {
                return isLoop ? dictionary[dictionary.Count - 1].Item2 : default;
            }

            var indexOf = dictionary.Keys.IndexOf(key);

            if (indexOf == -1) return DefaultValue();

            return indexOf > 0 ? dictionary[indexOf - 1].Item2 : DefaultValue();
        }

        public static TKey GetNextKey<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
            bool isLoop = false)
        {
            if (dictionary.Count == 0) return default;

            TKey DefaultValue()
            {
                return isLoop ? dictionary[0].Item1 : default;
            }

            var indexOf = dictionary.Keys.IndexOf(key);

            if (indexOf == -1) return DefaultValue();

            return indexOf >= dictionary.Count - 1 ? DefaultValue() : dictionary[indexOf + 1].Item1;
        }

        public static TKey GetPreviousKey<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
            bool isLoop = false)
        {
            if (dictionary.Count == 0) return default;

            TKey DefaultValue()
            {
                return isLoop ? dictionary[dictionary.Count - 1].Item1 : default;
            }

            var indexOf = dictionary.Keys.IndexOf(key);

            if (indexOf == -1) return isLoop ? dictionary[dictionary.Count - 1].Item1 : DefaultValue();

            return indexOf > 0 ? dictionary[indexOf - 1].Item1 : DefaultValue();
        }

        public static void AddOrSetValue<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
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

        public static void AddIfNotContains<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.TryGetValue(key, out _)) return;

            dictionary.Add(key, value);
        }

        public static void ReplaceValueWithKey<TKey, TValue>(this CustomDictionary<TKey, TValue> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.Keys.Contains(key))
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