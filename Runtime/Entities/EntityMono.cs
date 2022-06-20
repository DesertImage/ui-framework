using System;
using System.Collections;
using DesertImage.Events;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace DesertImage.ECS
{
    public class EntityMono : MonoBehaviour, IEntity
    {
        public event Action<IComponentHolder, IComponent> OnComponentAdded;
        public event Action<IComponentHolder, IComponent> OnComponentRemoved;
        public event Action<IComponentHolder, IComponent, IComponent> OnComponentPreUpdated;
        public event Action<IComponentHolder, IComponent> OnComponentUpdated;

        public event Action<IComponentHolder> OnDispose;

        public int Id => _localEntity?.Id ?? 0;

        public IComponent[] Components => _localEntity?.Components;

        [SerializeField] private bool autoInitialize;

        private IComponentWrapper[] _componentWrappers;

        protected IEntity LocalEntity
        {
            get
            {
#if DEBUG
                if (_localEntity == null)
                {
                    UnityEngine.Debug.LogError($"<b>[EntityMono]</b> entity {name} not initialized");
                    return default;
                }
#endif
                return _localEntity;
            }
        }

        private IEntity _localEntity;

        private IWorld _world;

        private void Awake()
        {
            _componentWrappers ??= GetComponents<IComponentWrapper>();
        }

        private IEnumerator Start()
        {
            while (!Core.Instance?.IsInitialized ?? true)
            {
                yield return null;
            }

            if (!autoInitialize) yield break;

            OnCreate();
        }

        #region COMPONENTS

        public IComponent Add(IComponent component)
        {
            return LocalEntity?.Add(component);
        }

        public TComponent Add<TComponent>() where TComponent : IComponent, new()
        {
            return LocalEntity == null ? default : LocalEntity.Add<TComponent>();
        }

        public IComponent Get(ushort id)
        {
            return LocalEntity?.Get(id);
        }

        public T Get<T>(ushort id) where T : IComponent
        {
            return LocalEntity == null ? default : LocalEntity.Get<T>(id);
        }

        public bool HasComponent(ushort id)
        {
            return LocalEntity?.HasComponent(id) ?? default;
        }

        public void Remove(ushort id)
        {
            LocalEntity?.Remove(id);
        }

        #endregion

        public void OnCreate()
        {
            _world ??= Core.Instance.Get<World>();

            _localEntity = _world.GetNewEntity();

            _componentWrappers ??= GetComponents<IComponentWrapper>();

            if ((_componentWrappers?.Length ?? 0) == 0) return;

            foreach (var componentWrapper in _componentWrappers)
            {
                componentWrapper.Link(this);
            }
        }

        public void ReturnToPool()
        {
            Dispose();
        }

        public void Dispose()
        {
            _localEntity = null;
        }

        public void ListenEvent<TEvent>(IListen listener)
        {
            _localEntity.ListenEvent<TEvent>(listener);
        }

        public void UnlistenEvent<TEvent>(IListen listener)
        {
            _localEntity.UnlistenEvent<TEvent>(listener);
        }

        public void SendEvent<TEvent>(TEvent @event)
        {
            _localEntity.SendEvent(@event);
        }
    }
}