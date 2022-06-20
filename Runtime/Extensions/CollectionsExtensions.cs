using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DesertImage.Extensions
{
    public static class CollectionsExtensions
    {
        public static T[] Shuffle<T>(this IEnumerable<T> array)
        {
            var rnd = new System.Random();

            return array.OrderBy(x => rnd.Next()).ToArray();
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            var rnd = new System.Random();

            return array.OrderBy(x => rnd.Next()).ToArray();
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var rnd = new System.Random();

            return list.OrderBy(x => rnd.Next()).ToList();
        }

        public static ReactiveCollection<T> ShuffleListToReactive<T>(this IEnumerable<T> array)
        {
            var rnd = new System.Random();

            return array.OrderBy(x => rnd.Next()).ToReactiveCollection();
        }

        public static bool IsHaveNullElements<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return true;

            foreach (var element in collection)
            {
                if (!element?.Equals(default) ?? false) continue;

                return true;
            }

            return false;
        }

        public static T[] GetSubArray<T>(this T[] array, int index, int length)
        {
            var result = new T[length];

            Array.Copy(array, index, result, 0, length);

            return result;
        }

        public static void AddRangeWithoutNulls<T>(this ICollection<T> collection, IEnumerable<T> addingCollection)
        {
            if (collection == null) return;

            foreach (var item in addingCollection)
            {
                if (item?.Equals(default) ?? false) continue;

                collection.Add(item);
            }
        }

        public static void ShiftLeft<T>(this T[] array, int startIndex)
        {
            for (var i = startIndex; i < array.Length - 1; i++)
            {
                array[i - 1] = array[i];

                array[i] = default;
            }
        }

        public static int IndexOf<T>(this T[] array, T element, IEqualityComparer<T> equalityComparer = null)
        {
            var index = -1;

            for (var i = 0; i < array.Length; i++)
            {
                var elem = array[i];

                // if (equalityComparer?.Equals(elem, default) ?? Equals(elem, default)) continue;

                var isEqual = equalityComparer?.Equals(elem, element) ?? elem?.Equals(element) ?? false;

                if (!isEqual) continue;

                index = i;

                break;
            }

            return index;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> compareExpression)
        {
            if (items == null) return -1;
            if (compareExpression == null) return -1;

            var index = 0;

            foreach (var item in items)
            {
                if (compareExpression(item)) return index;

                index++;
            }

            return -1;
        }

        public static int IndexOf<T>(this T[,] items, T targetItem, int firstIndex) where T : IEquatable<T>
        {
            if (items == null) return -1;

            for (var j = 0; j < items.GetLength(1); j++)
            {
                var item = items[firstIndex, j];

                if (item.Equals(targetItem)) continue;

                return j;
            }

            return -1;
        }

        public static int IndexOf<T>(this T[,] items, Func<T, bool> compareFunc, int firstIndex)
        {
            if (items == null) return -1;

            for (var j = 0; j < items.GetLength(1); j++)
            {
                var item = items[firstIndex, j];

                if (!compareFunc(item)) continue;

                return j;
            }

            return -1;
        }

        public static T[] Add<T>(this T[] array, T element)
        {
            Array.Resize(ref array, array.Length + 1);

            array[array.Length - 1] = element;

            return array;
        }

        public static T[] AddRange<T>(this T[] array, T[] elements)
        {
            if ((elements?.Length ?? 0) == 0) return array;

            var oldLength = array.Length;

            Array.Resize(ref array, array.Length + elements.Length);

            for (var i = oldLength; i <= array.Length - 1; i++)
            {
                array[i] = elements[i - oldLength];
            }

            return array;
        }

        public static void Copy<T>(this T[,] array, T[,] targetArray)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    targetArray[i, j] = array[i, j];
                }
            }
        }

        public static T GetNextElement<T>(this List<T> list, T element, bool isLooped = false)
        {
            var elem = default(T);

            if (list.Count == 0) return elem;

            var index = list.IndexOf(element);

            if (list.Count > 1 && index >= 0 && index < list.Count - 1)
            {
                elem = list[index + 1];
            }
            else if (isLooped && index == list.Count - 1)
            {
                elem = list[0];
            }

            return elem;
        }

        public static T GetNextElement<T>(this T[] array, T element, bool isLooped = false)
        {
            var elem = default(T);

            if (array.Length == 0) return elem;

            var index = array.IndexOf(element);

            if (index < 0 && array.Length > 0)
            {
                return array[0];
            }

            if (array.Length > 1 && index >= 0 && index < array.Length - 1)
            {
                elem = array[index + 1];
            }
            else if (isLooped && index == array.Length - 1)
            {
                elem = array[0];
            }

            return elem;
        }

        public static T GetPreviousElement<T>(this IList<T> list, T element, bool isLooped = false)
        {
            var elem = default(T);

            if (list.Count == 0) return elem;

            var index = list.IndexOf(element);

            if (list.Count > 1 && index > 0 && index <= list.Count - 1)
            {
                elem = list[index - 1];
            }
            else if (isLooped && index == 0)
            {
                elem = list[list.Count - 1];
            }

            return elem;
        }

        public static T GetPreviousElement<T>(this T[] array, T element, bool isLooped = false)
        {
            var elem = default(T);

            if (array.Length == 0) return elem;

            var index = array.IndexOf(element);

            if (array.Length > 1 && index > 0 && index <= array.Length - 1)
            {
                elem = array[index - 1];
            }
            else if (isLooped && index == 0)
            {
                elem = array[array.Length - 1];
            }

            return elem;
        }

        public static T GetRandomElement<T>(this IList<T> list)
        {
            var rand = default(T);

            if (list == null) return rand;

            if (list.Count > 0)
            {
                rand = list[Random.Range(0, list.Count)];
            }

            return rand;
        }

        public static T GetRandomElement<T>(this T[] array)
        {
            var rand = default(T);

            if (array == null) return rand;

            if (array.Length > 0)
            {
                rand = array[Random.Range(0, array.Length)];
            }

            return rand;
        }

        public static T GetRandomElement<T>(this T[,] array)
        {
            var rand = default(T);

            if (array == null) return rand;

            if (array.Length > 0)
            {
                rand = array[Random.Range(0, array.GetLength(0)), Random.Range(0, array.GetLength(1))];
            }

            return rand;
        }

        public static T GetRandomElement<T>(this T[,] array, int minX, int maxX, int minY, int maxY)
        {
            var rand = default(T);

            if (array == null) return rand;

            if (array.Length > 0)
            {
                rand = array[Random.Range(minX, maxX), Random.Range(minY, maxY)];
            }

            return rand;
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            var rand = default(T);

            if (list == null) return rand;

            if (list.Count > 0)
            {
                rand = list[Random.Range(0, list.Count)];
            }

            return rand;
        }

        public static T GetRandomElement<T>(this List<T> list, int startIndex, int endIndex)
        {
            var rand = default(T);

            if (list.Count <= 0 || startIndex > endIndex) return rand;

            rand = list[Random.Range(startIndex, endIndex < list.Count ? endIndex : list.Count - 1)];

            return rand;
        }

        public static T GetRandomElement<T>(this List<T> list, T exceptElement)
        {
            var rand = default(T);

            if (list.Count <= 0) return rand;

            rand = list[Random.Range(0, list.Count)];

            if (rand.Equals(exceptElement) && list.Count > 1)
            {
                rand = GetRandomElement(list, exceptElement);
            }

            return rand;
        }

        public static T GetRandomElement<T>(this List<T> list, List<T> exceptList)
        {
            var rand = default(T);

            if (list.Count <= 0) return rand;

            rand = list[Random.Range(0, list.Count)];

            if (exceptList.Contains(rand) && exceptList.Count < list.Count)
            {
                rand = list.GetRandomElement(exceptList);
            }

            return rand;
        }

        public static T GetRandomElement<T>(this T[] array, IEnumerable<T> exceptCollection)
        {
            var rand = default(T);

            if (array.Length <= 0) return rand;

            var arrayForRnd = array.Except(exceptCollection).ToArray();

            return arrayForRnd.Length == 0 ? rand : arrayForRnd[Random.Range(0, arrayForRnd.Length)];
        }

        public static T GetRandomElement<T>(this T[] list, T exceptElement)
        {
            var rand = default(T);

            if (list.Length <= 0) return rand;

            rand = list[Random.Range(0, list.Length)];

            if (rand.Equals(exceptElement) && list.Length > 1)
            {
                rand = GetRandomElement(list, exceptElement);
            }

            return rand;
        }

        public static T GetRandomElement<T>(this T[] list, int minIndex, int maxIndex, T exceptElement)
        {
            var rand = default(T);

            if (list.Length <= 0) return rand;

            rand = list[Random.Range(minIndex, maxIndex)];

            if (rand.Equals(exceptElement) && list.Length > 1)
            {
                rand = GetRandomElement(list, exceptElement);
            }

            return rand;
        }

        public static bool IsEqual<T>(this T[,] array, T[,] compareArray)
        {
            var isEqual = true;

            if (array.Length != compareArray.Length) return false;

            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (compareArray[i, j].Equals(array[i, j])) continue;

                    isEqual = false;
                    break;
                }
            }

            return isEqual;
        }

        public static bool IsEqual<T>(this T[] array, T[] compareArray, Func<T, T, bool> equalityFunc)
        {
            if (ReferenceEquals(array, compareArray)) return true;

            if (array.Length != compareArray.Length) return false;

            for (var i = 0; i < array.GetLength(0); i++)
            {
                if (!equalityFunc.Invoke(array[i], compareArray[i])) continue;

                return false;
            }

            return true;
        }

        public static bool IsEqualWithoutOrder<T>(this T[] array, T[] compareArray)
        {
            if (array.Length != compareArray.Length) return false;

            for (var i = 0; i < array.GetLength(0); i++)
            {
                if (Contains(compareArray, array[i])) continue;

                return false;
            }

            return true;
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count <= 0) return;

            list.RemoveAt(list.Count - 1);
        }

        public static T GetLast<T>(this List<T> list)
        {
            var rand = default(T);

            return list.Count <= 0 ? rand : list[list.Count - 1];
        }

        public static void MoveDown<T>(this List<T> collection, T item)
        {
            var index = collection.IndexOf(item);

            if (index < 0 || index >= collection.Count - 1) return;

            var nextElement = collection[index + 1];

            collection[index + 1] = item;

            collection[index] = nextElement;
        }

        public static void MoveUp<T>(this List<T> collection, T item)
        {
            var index = collection.IndexOf(item);

            if (index < 0 || index == 0) return;

            var nextElement = collection[index - 1];

            collection[index - 1] = item;

            collection[index] = nextElement;
        }

        public static void AddIfNotContains<T>(this ICollection<T> collection, T item)
        {
            if (collection.Contains(item)) return;

            collection.Add(item);
        }

        public static void RemoveIfContains<T>(this ICollection<T> collection, T item)
        {
            if (!collection.Contains(item)) return;

            collection.Remove(item);
        }

        public static void RemoveIfContains<T>(this ICollection<T> collection, T item, Func<T, T, bool> check)
        {
            foreach (var collectionItem in collection)
            {
                if (!check.Invoke(item, collectionItem)) continue;

                collection.Remove(collectionItem);

                break;
            }
        }

        public static T[] AddIfNotContains<T>(this T[] array, T item)
        {
            return array.Contains(item) ? array : array.Add(item);
        }

        public static bool Contains<T>(this T[] array, T targetItem)
        {
            if (array == null) return false;

            for (var i = 0; i < array.Length; i++)
            {
                if (!Equals(array[i], targetItem)) continue;

                return true;
            }

            return false;
        }

        public static bool Contains<T>(this T[] array, T targetItem, Func<T, T, bool> checkFunc)
        {
            if (array == null) return false;

            for (var i = 0; i < array.Length; i++)
            {
                if (!checkFunc.Invoke(targetItem, array[i])) continue;

                return true;
            }

            return false;
        }

        public static bool Contains<T>(this T[] array, T targetItem, IEqualityComparer<T> comparer)
        {
            if (array == null) return false;

            for (var i = 0; i < array.Length; i++)
            {
                if (!comparer.Equals(array[i], targetItem)) continue;

                return true;
            }

            return false;
        }

        public static void CopyTo<T>(this T[] sourceArray, T[] targetArray)
        {
            for (var i = 0; i < sourceArray.Length; i++)
            {
                if (i >= targetArray.Length) break;

                targetArray[i] = sourceArray[i];
            }
        }

        public static T[,] ConvertTo2DimensionArray<T>(this IList<IList<T>> collection, int maxHeight = 150,
            int maxWidth = 150, int startColumn = 0)
        {
            var array = new T[Mathf.Min(collection.Count, maxHeight), Mathf.Min(collection.GetMaxLength(), maxWidth)];

            for (var i = startColumn; i < collection.Count; i++)
            {
                if (i >= maxHeight) break;

                var raws = collection[i];

                for (var j = 0; j < raws.Count; j++)
                {
                    if (j >= maxWidth) break;

                    var column = raws[j];

                    array[i, j] = column;
                }
            }

            return array;
        }

        public static int GetMaxLength<T>(this IList<IList<T>> collection)
        {
            var maxLength = -1;

            if (collection == null) return maxLength;

            foreach (var element in collection)
            {
                if (maxLength >= element.Count) continue;

                maxLength = element.Count;
            }

            return maxLength;
        }

        public static void Refill<T>(this IList<T> collection, int count, Func<T, int, T> setupAction,
            Func<T> addFunc = null,
            Action<T> removeAction = null)
        {
            var newLength = count;

            if (newLength == 0 || collection == null)
            {
                collection?.Clear();

                return;
            }

            var originalCount = collection.Count;

            var countToRemove = collection.Count - count;
            for (var i = collection.Count - 1; i >= 0 && i >= (originalCount - countToRemove); i--)
            {
                removeAction?.Invoke(collection[i]);
                collection.RemoveAt(i);
            }

            var countToAdd = count - collection.Count;
            for (var i = 0; i < countToAdd && i >= 0; i++)
            {
                collection.Add(addFunc != null ? addFunc.Invoke() : default);
            }

            for (var i = 0; i < collection.Count && i >= 0; i++)
            {
                if (setupAction == null) continue;

                collection[i] = setupAction.Invoke(collection[i], i);
            }
        }

        public static void Refill<T>(this IList<T> collection, int count, Func<T, int, T> setupAction,
            Func<int, T> addFunc = null,
            Action<int> removeAction = null)
        {
            var newLength = count;

            if (collection == null) return;

            if (newLength == 0)
            {
                if (collection.Count > 0)
                {
                    for (var i = collection.Count - 1; i >= 0; i--)
                    {
                        removeAction?.Invoke(i);
                    }
                }

                collection?.Clear();

                return;
            }

            var countToRemove = collection.Count - count;

            var targetCount = collection.Count - countToRemove;

            for (var i = collection.Count - 1; i > 0 && i >= targetCount; i--)
            {
                removeAction?.Invoke(i);
                collection.RemoveAt(i);
            }

            var countToAdd = count - collection.Count;
            for (var i = 0; i < countToAdd; i++)
            {
                collection.Add(addFunc != null ? addFunc.Invoke(i) : default);
            }

            for (var i = 0; i < collection.Count; i++)
            {
                if (setupAction == null) continue;

                collection[i] = setupAction.Invoke(collection[i], i);
            }
        }

        public static bool IsLastElement<T>(this List<T> list, T element)
        {
            var index = list.IndexOf(element);
            return index == list.Count - 1;
        }

        public static Vector3 Clamp(this Vector3 array, Vector2 xMinMax, Vector2 yMinMax, Vector2 zMinMax)
        {
            return new Vector3
            (
                Mathf.Clamp(array.x, xMinMax.x, xMinMax.y),
                Mathf.Clamp(array.y, yMinMax.x, yMinMax.y),
                Mathf.Clamp(array.z, zMinMax.x, zMinMax.y)
            );
        }

        public static Vector2 Clamp(this Vector2 array, Vector2 xMinMax, Vector2 yMinMax)
        {
            return new Vector2
            (
                Mathf.Clamp(array.x, xMinMax.x, xMinMax.y),
                Mathf.Clamp(array.y, yMinMax.x, yMinMax.y)
            );
        }
    }
}