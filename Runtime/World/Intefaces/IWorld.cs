using System;
using System.Collections.Generic;
using External;
using Group;

namespace DesertImage.ECS
{
    public interface IWorld
    {
        event Action<IEntity> OnEntityAdded;
        event Action<IEntity> OnEntityRemoved;
        
        event Action<IEntity, IComponent> OnEntityComponentAdded;
        event Action<IEntity, IComponent> OnEntityComponentRemoved;
        event Action<IEntity, IComponent, IComponent> OnEntityComponentPreUpdated;
        event Action<IEntity, IComponent> OnEntityComponentUpdated;

        event Action<IMatcher, EntityGroup> OnGroupAdded;
        event Action<IMatcher, EntityGroup> OnGroupRemoved;

        HashSet<IEntity> Entities { get; }

        CustomDictionary<IMatcher, EntityGroup> Groups { get; }
        CustomDictionary<ushort, IMatcher> Matchers { get; }
        CustomDictionary<ushort, List<(IMatcher, EntityGroup)>> ComponentGroups { get; }

        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);

        IEntity GetNewEntity();
        IEntity GetNewEntity(Action<IEntity> setup);

        EntityGroup GetGroup(IMatcher matcher);
        EntityGroup GetGroup(ushort componentId);
        EntityGroup GetGroup(ushort[] componentIds);
    }
}