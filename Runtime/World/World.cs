using System;
using System.Collections.Generic;
using DesertImage.Events;
using DesertImage.Extensions;
using DesertImage.Pools;
using External;
using Group;

namespace DesertImage.ECS
{
    public class World : IWorld, IListen<ComponentAddedEvent>,
        IListen<ComponentRemovedEvent>,
        IListen<ComponentPreUpdatedEvent>,
        IListen<ComponentUpdatedEvent>
    {
        public event Action<IEntity> OnEntityAdded;
        public event Action<IEntity> OnEntityRemoved;

        public event Action<IEntity, IComponent> OnEntityComponentAdded;
        public event Action<IEntity, IComponent> OnEntityComponentRemoved;
        public event Action<IEntity, IComponent, IComponent> OnEntityComponentPreUpdated;
        public event Action<IEntity, IComponent> OnEntityComponentUpdated;

        public event Action<IMatcher, EntityGroup> OnGroupAdded;
        public event Action<IMatcher, EntityGroup> OnGroupRemoved;

        public HashSet<IEntity> Entities { get; } = new HashSet<IEntity>();

        public CustomDictionary<IMatcher, EntityGroup> Groups { get; } =
            new CustomDictionary<IMatcher, EntityGroup>(new MatchersComparer());

        public CustomDictionary<ushort, IMatcher> Matchers { get; } = new CustomDictionary<ushort, IMatcher>();

        public CustomDictionary<ushort, List<(IMatcher, EntityGroup)>> ComponentGroups { get; } =
            new CustomDictionary<ushort, List<(IMatcher, EntityGroup)>>();

        private readonly Pool<IEntity> EntitiesPool = new EntityPool();
        private readonly Pool<EntityGroup> GroupsPool = new EntityGroupsPool();

        public void AddEntity(IEntity entity)
        {
            Entities.Add(entity);

            entity.ListenEvent<ComponentAddedEvent>(this);
            entity.ListenEvent<ComponentRemovedEvent>(this);
            entity.ListenEvent<ComponentPreUpdatedEvent>(this);
            entity.ListenEvent<ComponentUpdatedEvent>(this);

            OnEntityAdded?.Invoke(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!Entities.Contains(entity)) return;

            entity.UnlistenEvent<ComponentAddedEvent>(this);
            entity.UnlistenEvent<ComponentRemovedEvent>(this);
            entity.UnlistenEvent<ComponentPreUpdatedEvent>(this);
            entity.UnlistenEvent<ComponentUpdatedEvent>(this);

            Entities.Remove(entity);

            OnEntityRemoved?.Invoke(entity);
        }

        public IEntity GetNewEntity()
        {
            var entity = GetEntity();

            AddEntity(entity);

            return entity;
        }

        public IEntity GetNewEntity(Action<IEntity> setup)
        {
            var entity = GetEntity();

            AddEntity(entity);

            setup?.Invoke(entity);

            return entity;
        }

        private IEntity GetEntity()
        {
            var entity = EntitiesPool.GetInstance();

            entity.OnDispose += EntityOnDispose;

            return entity;
        }

        public EntityGroup GetGroup(IMatcher matcher)
        {
            if (Groups.TryGetValue(matcher, out var group))
            {
                return group;
            }

            group = GroupsPool.GetInstance();

            AddGroup(matcher, group);

            return group;
        }

        public EntityGroup GetGroup(ushort componentId)
        {
            for (var i = 0; i < Groups.Count; i++)
            {
                var (matcher, group) = Groups[i];

                if ((matcher.ComponentIds?.Length ?? 0) > 1) continue;
                if (!matcher.IsContainsComponent(componentId)) continue;

                return group;
            }

            var targetGroup = GroupsPool.GetInstance();

            AddGroup(Match.AllOf(componentId), targetGroup);

            return targetGroup;
        }

        public EntityGroup GetGroup(ushort[] componentIds)
        {
            for (var i = 0; i < Groups.Count; i++)
            {
                var (matcher, group) = Groups[i];

                if (matcher.ComponentIds?.Length != componentIds.Length) continue;
                if (!matcher.ComponentIds.IsEqual(componentIds, (id1, id2) => id1 == id2)) continue;

                return group;
            }

            var targetGroup = GroupsPool.GetInstance();

            AddGroup(Match.AllOf(componentIds), targetGroup);

            return targetGroup;
        }

        private void AddGroup(IMatcher matcher, EntityGroup group)
        {
            Groups.Add
            (
                matcher,
                group
            );

            Matchers.Add(group.Id, matcher);

            foreach (var componentId in matcher.ComponentIds)
            {
                if (ComponentGroups.TryGetValue(componentId, out var groupInfos))
                {
                    groupInfos.Add((matcher, group));
                }
                else
                {
                    ComponentGroups.Add(componentId, new List<(IMatcher, EntityGroup)> { (matcher, group) });
                }
            }

            OnGroupAdded?.Invoke(matcher, group);
        }

        #region CALLBACKS

        private void EntityOnComponentRemoved(IComponentHolder componentHolder, IComponent component)
        {
            OnEntityComponentAdded?.Invoke((IEntity)componentHolder, component);
        }

        private void EntityOnComponentAdded(IComponentHolder componentHolder, IComponent component)
        {
            OnEntityComponentRemoved?.Invoke((IEntity)componentHolder, component);
        }

        private void EntityOnPreUpdated(IComponentHolder componentHolder, IComponent previous, IComponent future)
        {
            OnEntityComponentPreUpdated?.Invoke((IEntity)componentHolder, previous, future);
        }

        private void EntityOnUpdated(IComponentHolder componentHolder, IComponent component)
        {
            OnEntityComponentUpdated?.Invoke((IEntity)componentHolder, component);
        }

        private void EntityOnDispose(IComponentHolder componentHolder)
        {
            var entity = componentHolder as IEntity;

            // ReSharper disable once PossibleNullReferenceException
            entity.OnDispose -= EntityOnDispose;

            RemoveEntity(entity);

            EntitiesPool.ReturnInstance(entity);
        }

        public void HandleCallback(ComponentAddedEvent arguments)
        {
            EntityOnComponentAdded(arguments.Holder, arguments.Value);
        }

        public void HandleCallback(ComponentRemovedEvent arguments)
        {
            EntityOnComponentRemoved(arguments.Holder, arguments.Value);
        }

        public void HandleCallback(ComponentPreUpdatedEvent arguments)
        {
            EntityOnPreUpdated(arguments.Holder, arguments.PreviousValue, arguments.FutureValue);
        }

        public void HandleCallback(ComponentUpdatedEvent arguments)
        {
            EntityOnUpdated(arguments.Holder, arguments.Value);
        }

        #endregion
    }
}