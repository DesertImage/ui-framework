using System;
using System.Collections;
using System.Collections.Generic;
using DesertImage.Extensions;

namespace DesertImage.External
{
    public interface ICustomList
    {
    }

    public interface ICustomList<T> : IList<T>, ICustomList
    {
    }

    [Serializable]
    public class CustomList<T> : ICustomList<T>
    {
        public int Count { get; private set; }
        public bool IsReadOnly { get; }

        public T[] Values { get; private set; }

        private int _additionalStep;

        public CustomList(int preGeneratedCount = 10, int additionalStep = 3)
        {
            Values = new T[preGeneratedCount];

            _additionalStep = additionalStep;
            if (additionalStep <= 0)
            {
                _additionalStep = 1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new List<T>.Enumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (Count >= Values.Length)
            {
                var values = Values;

                Array.Resize(ref values, Values.Length + _additionalStep);

                Values = values;
            }

            Values[Count] = item;

            Count++;
        }

        public bool Remove(T item)
        {
            var index = Values.IndexOf(item);

            if (index == -1) return false;

            Values[index] = default;

            Values.ShiftLeft(index + 1);

            Count--;

            return true;
        }

        public void RemoveAt(int index)
        {
            Values[index] = default;

            Values.ShiftLeft(index + 1);

            Count--;
        }

        public void Clear()
        {
            for (var i = 0; i < Values.Length; i++)
            {
                Values[i] = default;
            }

            Count = 0;
        }

        public bool Contains(T item)
        {
            return Values.IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = arrayIndex; i < array.Length; i++)
            {
                array[i] = Values[i];
            }
        }

        public int IndexOf(T item)
        {
            return Values.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public T this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }
    }
}