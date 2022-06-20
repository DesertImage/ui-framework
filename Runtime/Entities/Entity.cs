using System;
using DesertImage.Events;

namespace DesertImage.ECS
{
    public class Entity : EventUnit, IEntity,
        IListen<ComponentRemovedEvent>,
        IListen<ComponentPreUpdatedEvent>,
        IListen<ComponentUpdatedEvent>
    {
        public event Action<IComponentHolder, IComponent> OnComponentAdded;
        public event Action<IComponentHolder, IComponent> OnComponentRemoved;
        public event Action<IComponentHolder, IComponent, IComponent> OnComponentPreUpdated;
        public event Action<IComponentHolder, IComponent> OnComponentUpdated;

        public event Action<IComponentHolder> OnDispose;

        public int Id { get; }

        public IComponent[] Components { get; }

        private int _componentsCount;

        public Entity(int id, int componentsBuffer = ECSSettings.ComponentsCount)
        {
            Id = id;

            Components = new IComponent[componentsBuffer];
        }

        #region COMPONENTS

        public IComponent Add(IComponent component)
        {
            if (Components[component.Id] == null)
            {
                _componentsCount++;
            }

            Components[component.Id] = component;

            // OnComponentAdded?.Invoke(this, component);

            EventsManager.Send(new ComponentAddedEvent
            {
                Holder = this,
                Value = component
            });

            // component.OnPreUpdated += ComponentOnPreUpdated;
            // component.OnUpdated += ComponentOnUpdated;
            component.ListenEvent<ComponentPreUpdatedEvent>(this);
            component.ListenEvent<ComponentUpdatedEvent>(this);

            return component;
        }

        public TComponent Add<TComponent>() where TComponent : IComponent, new()
        {
            var component = ComponentsTool.GetInstanceFromPool<TComponent>();

            return (TComponent)Add(component);
        }

        public void Remove(ushort id)
        {
            var component = Components[id];

            Components[id] = null;

            if (component == null) return;

            // component.OnPreUpdated -= ComponentOnPreUpdated;
            // component.OnUpdated -= ComponentOnUpdated;

            // OnComponentRemoved?.Invoke(this, component);

            component.UnlistenEvent<ComponentPreUpdatedEvent>(this);
            component.UnlistenEvent<ComponentUpdatedEvent>(this);

            EventsManager.Send(new ComponentRemovedEvent
            {
                Holder = this,
                Value = component
            });

            component.ReturnToPool();

            _componentsCount--;

            if (_componentsCount == 0)
            {
                ReturnToPool();
            }
        }

        public IComponent Get(ushort id)
        {
            return Components[id];
        }

        public T Get<T>(ushort id) where T : IComponent
        {
            return (T)Get(id);
        }

        public bool HasComponent(ushort id)
        {
            return Components[id] != null;
        }

        private void ClearComponents()
        {
            for (var i = 0; i < Components.Length; i++)
            {
                Components[i]?.ReturnToPool();
                Components[i] = null;
            }
        }

        private void ComponentOnPreUpdated(IComponent component, IComponent newValues)
        {
            // OnComponentPreUpdated?.Invoke(this, component, newValues);

            EventsManager.Send(new ComponentPreUpdatedEvent
            {
                Holder = this,
                PreviousValue = component,
                FutureValue = newValues
            });
        }

        private void ComponentOnUpdated(IComponent component)
        {
            // OnComponentUpdated?.Invoke(this, component);

            EventsManager.Send(new ComponentUpdatedEvent
            {
                Holder = this,
                Value = component
            });
        }

        #endregion

        public void OnCreate()
        {
        }

        public void ReturnToPool()
        {
            EventsManager.Clear();

            Dispose();
        }

        public void Dispose()
        {
            ClearComponents();

            OnDispose?.Invoke(this);

            OnComponentAdded = null;
            OnComponentRemoved = null;
            OnComponentUpdated = null;

            OnDispose = null;
        }

        public void HandleCallback(ComponentRemovedEvent arguments)
        {
            EventsManager.Send
            (
                new ComponentRemovedEvent
                {
                    Holder = this,
                    Value = arguments.Value,
                }
            );
        }

        public void HandleCallback(ComponentPreUpdatedEvent arguments)
        {
            EventsManager.Send
            (
                new ComponentPreUpdatedEvent
                {
                    Holder = this,
                    PreviousValue = arguments.PreviousValue,
                    FutureValue = arguments.FutureValue
                }
            );
        }

        public void HandleCallback(ComponentUpdatedEvent arguments)
        {
            EventsManager.Send
            (
                new ComponentUpdatedEvent
                {
                    Holder = this,
                    Value = arguments.Value,
                }
            );
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}