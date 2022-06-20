using System;
using System.Collections;
using System.Collections.Generic;
using DesertImage.ECS;
using DesertImage.Extensions;
using UnityEngine;

namespace External
{
    [Serializable]
    public class CustomDictionary<TKey, TValue>
    {
        [Serializable]
        private struct Entry
        {
            public int Index;

            public TKey Key;
            public TValue Value;
        }

        public int Count => _count;

        public TKey[] Keys => keys;
        public TValue[] Values => values;

        private Dictionary<TKey, Entry> _dictionary;

        [SerializeField] private TKey[] keys;
        [SerializeField] private TValue[] values;

        private int _count;

        private TKey _defaultKey;
        private TValue _defaultValue;

        public ValueTuple<TKey, TValue> this[int index] => (keys[index], values[index]);

        public TValue this[TKey key]
        {
            get => TryGetValue(key, out var value) ? value : _defaultValue;

            set
            {
                if (_dictionary.TryGetValue(key, out var entry))
                {
                    var index = entry.Index;

                    values[index] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public TKey this[TValue value]
        {
            get
            {
                var index = values.IndexOf(value);

                return index >= 0 ? keys[index] : default;
            }
        }

        private int _fillStep;

        public CustomDictionary
        (
            int cachedElementsCount = 10,
            int fillStep = 3,
            TKey defaultKey = default,
            TValue defaultValue = default
        )
        {
            _dictionary = new Dictionary<TKey, Entry>();

            keys = new TKey[cachedElementsCount];
            values = new TValue[cachedElementsCount];

            _fillStep = fillStep;

            _defaultKey = defaultKey;
            _defaultValue = defaultValue;

            for (var i = 0; i < cachedElementsCount; i++)
            {
                keys[i] = defaultKey;
                values[i] = defaultValue;
            }
        }

        public CustomDictionary(IEqualityComparer<TKey> equalityComparer)
        {
            _dictionary = new Dictionary<TKey, Entry>(equalityComparer);

            keys = new TKey[10];
            values = new TValue[10];

            _fillStep = 3;
        }

        public CustomDictionary(TKey defaultKey, TValue defaultValue = default)
        {
            _dictionary = new Dictionary<TKey, Entry>();

            keys = new TKey[10];
            values = new TValue[10];

            _fillStep = 3;

            _defaultKey = defaultKey;
            _defaultValue = defaultValue;

            for (var i = 0; i < 10; i++)
            {
                keys[i] = defaultKey;
                values[i] = defaultValue;
            }
        }

        public CustomDictionary(TKey[] keys, TValue[] values, int fillStep = 3, TKey defaultKey = default,
            TValue defaultValue = default)
        {
            _dictionary = new Dictionary<TKey, Entry>();

            this.keys = keys;
            this.values = values;

            _fillStep = fillStep;

            _count = keys?.Length ?? 0;

            _defaultKey = defaultKey;
            _defaultValue = defaultValue;
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add
            (
                key,
                new Entry
                {
                    Index = _count,
                    Key = key,
                    Value = value
                }
            );

            if (_count == 0 || keys.Length <= _count)
            {
                IncSize(_fillStep);
            }

            keys[_count] = key;
            values[_count] = value;

            _count++;

            // if (count < keys.Length) return;

            // IncSize(_fillStep);
        }

        public void Remove(TKey key)
        {
            Remove(key, keys.IndexOf(key));
        }

        private void Remove(TKey key, int index)
        {
            _dictionary.Remove(key);

            if (index == -1 || _count == 0) return;

            keys[index] = _defaultKey;
            values[index] = _defaultValue;

            keys.ShiftLeft(index + 1);
            values.ShiftLeft(index + 1);

            _count--;
        }

        private void IncSize(int step)
        {
            step = Mathf.Max(1, step);

            var newKeys = keys;
            var newValues = values;

            Array.Resize(ref newKeys, _count + step);
            Array.Resize(ref newValues, _count + step);

            for (var i = _count; i < _count + step; i++)
            {
                newKeys[i] = _defaultKey;
                newValues[i] = _defaultValue;
            }

            keys = newKeys;
            values = newValues;
        }

        public void Clear()
        {
            _dictionary.Clear();

            _count = 0;

            for (var i = 0; i < keys.Length; i++)
            {
                keys[i] = _defaultKey;
            }

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = _defaultValue;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_dictionary.TryGetValue(key, out var entry))
            {
                value = entry.Value;

                return true;
            }

            value = default;

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }
    }
}