using System;
using System.Collections.Generic;
using DesertImage;
using DesertImage.ECS;
using DesertImage.Events;
using DesertImage.External;

namespace Group
{
    public class EntityGroup : EventUnit, IGroup<IEntity>
    {
        public ushort Id { get; set; }

        public event Action<IGroup, IEntity> OnEntityAdded;
        public event Action<IGroup, IEntity> OnEntityRemoved;
        public event Action<IGroup, IEntity, IComponent, IComponent> OnEntityPreUpdated;
        public event Action<IGroup, IEntity, IComponent> OnEntityUpdated;

        public CustomList<IEntity> Entities;

        private readonly HashSet<IEntity> _entitiesHashSet = new HashSet<IEntity>();

        private bool _isDisposing;

        public void Add(IEntity entity)
        {
            _entitiesHashSet.Add(entity);
            Entities.Add(entity);

            OnEntityAdded?.Invoke(this, entity);
        }

        public void Remove(IEntity entity)
        {
            if (!Contains(entity)) return;

            if (!_isDisposing)
            {
                _entitiesHashSet.Remove(entity);
                Entities.Remove(entity);
            }

            OnEntityRemoved?.Invoke(this, entity);
        }

        public bool Contains(IEntity entity)
        {
            return _entitiesHashSet.Contains(entity);
        }

        public void OnCreate()
        {
        }

        public void ReturnToPool()
        {
            Dispose();
        }

        public void PreUpdate(IEntity entity, IComponent component, IComponent newValues)
        {
            OnEntityPreUpdated?.Invoke(this, entity, component, newValues);
        }

        public void Update(IEntity entity, IComponent component)
        {
            OnEntityUpdated?.Invoke(this, entity, component);
        }

        public void Dispose()
        {
            _isDisposing = true;

            foreach (var entity in Entities)
            {
                Remove(entity);
            }

            _entitiesHashSet.Clear();
            Entities.Clear();

            _isDisposing = false;
        }
    }
}