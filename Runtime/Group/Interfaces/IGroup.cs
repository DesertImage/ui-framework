using System;
using DesertImage;
using DesertImage.ECS;

namespace Group
{
    public interface IGroup : IPoolable, IDisposable
    {
        ushort Id { get; }
    }

    public interface IGroup<TItem> : IGroup
    {
        event Action<IGroup, IEntity> OnEntityAdded;
        event Action<IGroup, IEntity> OnEntityRemoved;
        event Action<IGroup, IEntity, IComponent, IComponent> OnEntityPreUpdated;
        event Action<IGroup, IEntity, IComponent> OnEntityUpdated;

        void Add(TItem entity);
        void Remove(TItem entity);
        void PreUpdate(TItem entity, IComponent component, IComponent newValues);
        void Update(TItem entity, IComponent component);
        bool Contains(TItem entity);
    }
}