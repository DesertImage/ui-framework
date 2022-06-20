using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DesertImage.Extensions;
using DesertImage.Managers;
using UniRx;

namespace DesertImage.ECS
{
    public class Core : IStart, IDisposable
    {
        public static Core Instance { get; private set; }

        public bool IsInitialized { get; private set; }

        private readonly Dictionary<int, object> _data = new Dictionary<int, object>();

        private readonly List<IStart> _starts = new List<IStart>();

        public Core()
        {
            Instance = this;
        }

        public virtual void OnStart()
        {
            foreach (var start in _starts)
            {
                start.OnStart();
            }

            IsInitialized = true;
        }

        #region ADD

        /// <summary>
        /// Add object to Core
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Add<T>(T instance)
        {
            var (isAdded, _) = AddProcess(instance);

            if (!isAdded) return instance;

            if (instance is IAsyncAwake asyncAwake)
            {
                Observable.FromMicroCoroutine(asyncAwake.OnAsyncAwake).Subscribe();
            }

            if (instance is IStart start)
            {
                _starts.Add(start);
            }

            return instance;
        }

        /// <summary>
        /// Add object to Core
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerator AddAsync<T>(T instance)
        {
            var (isAdded, _) = AddProcess(instance);

            if (!isAdded) yield break;

            if (instance is IAsyncAwake asyncAwake)
            {
                var process = asyncAwake.OnAsyncAwake();

                while (process.MoveNext())
                {
                    yield return null;
                }
            }

            if (instance is IStart start)
            {
                _starts.Add(start);
            }

            yield return null;
        }

        /// <summary>
        /// Add object to Core. Using pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Add<T>() where T : new()
        {
            return Add(new T());
        }

        public bool IsHave<T>()
        {
            return Instance._data.TryGetValue(HashCodeTypeTool.GetCachedHashCode<T>(), out _);
        }

        private (bool, T) AddProcess<T>(T instance)
        {
            if (instance is ISwitchable switchable) switchable.Activate();

            if (instance is IAwake awake) awake.OnAwake();

            if (IsHave<ManagerUpdate>())
            {
                var managerUpdate = Get<ManagerUpdate>();

                if (instance is ITick tick)
                {
                    managerUpdate.Add(tick);
                }

                if (instance is ITickLate tickLate)
                {
                    managerUpdate.Add(tickLate);
                }

                if (instance is ITickFixed tickFixed)
                {
                    managerUpdate.Add(tickFixed);
                }
            }

            var hash = HashCodeTypeTool.GetCachedHashCode<T>();

            _data.Add(hash, instance);

            return (true, instance);
        }

        #endregion

        /// <summary>
        /// Remove object from Core
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>()
        {
            var hash = HashCodeTypeTool.GetCachedHashCode<T>();

            if (!_data.ContainsKey(hash)) return;

            var disposable = _data[hash] as IDisposable;

            disposable?.Dispose();

            _data.Remove(hash);
        }

        /// <summary>
        /// Get object from Core
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            var hash = HashCodeTypeTool.GetCachedHashCode<T>();

            if (!Instance._data.TryGetValue(hash, out var obj))
            {
#if DEBUG
                UnityEngine.Debug.LogWarning($"[Core] there is no component {typeof(T)}");
#endif
            }

            return (T)obj;
        }

        /// <summary>
        /// Replace object with new instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        public void Replace<T>(T instance)
        {
            var hash = HashCodeTypeTool.GetCachedHashCode<T>();

            if (Instance._data.TryGetValue(hash, out var obj))
            {
                Instance._data[hash] = instance;
            }
        }

        public virtual void Dispose()
        {
            // Instance = null;

            foreach (var value in _data.Values.Reverse())
            {
                var disposable = value as IDisposable;

                disposable?.Dispose();
            }
        }
    }
}