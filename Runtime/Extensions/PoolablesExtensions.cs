using System;
using System.Collections.Generic;
using DesertImage;

namespace Framework.Extensions
{
    public static class PoolablesExtensions
    {
        public static void RefillPoolables<T>(this IList<T> collection, T[] newCollection, Func<T> addFunc,
            Action<T> setupAction) where T : IPoolable
        {
            var newLength = newCollection?.Length ?? 0;

            if (newLength == 0 || collection == null)
            {
                collection?.ClearPoolables();

                return;
            }

            var countToRemove = collection.Count - newCollection.Length;
            for (var i = collection.Count - 1; i >= 0 && i >= countToRemove; i--)
            {
                collection[i].ReturnToPool();
                collection.RemoveAt(i);
            }

            var countToAdd = newCollection.Length - collection.Count;
            for (var i = 0; i < countToAdd; i++)
            {
                collection.Add(addFunc.Invoke());
            }
        }

        public static void RefillPoolables<T>(this IList<T> collection, int count, Func<T> addFunc,
            Action<T, int> setupAction) where T : IPoolable
        {
            var newLength = count;

            if (newLength == 0 || collection == null)
            {
                collection?.ClearPoolables();

                return;
            }

            var countToRemove = collection.Count - count;
            for (var i = collection.Count - 1; i >= 0 && i >= countToRemove; i--)
            {
                collection[i].ReturnToPool();
                collection.RemoveAt(i);
            }

            var countToAdd = count - collection.Count;
            for (var i = 0; i < countToAdd; i++)
            {
                collection.Add(addFunc.Invoke());
            }

            for (var i = 0; i < collection.Count; i++)
            {
                setupAction?.Invoke(collection[i], i);
            }
        }

        public static void ClearPoolables<T>(this IList<T> collection) where T : IPoolable
        {
            foreach (var poolable in collection)
            {
                poolable.ReturnToPool();
            }

            collection.Clear();
        }

        public static void ClearPoolables<T>(this IList<T> collection, Func<T, bool> conditionFunc) where T : IPoolable
        {
            foreach (var poolable in collection)
            {
                if (!(conditionFunc?.Invoke(poolable) ?? true)) continue;

                poolable.ReturnToPool();
            }

            collection.Clear();
        }
    }
}