using System;
using DesertImage.Events;
using DesertImage.Extensions;

namespace DesertImage.ECS
{
    [Serializable]
    public class DataComponent<T> : EventUnit, IDataComponent, IDisposable where T : DataComponent<T>
    {
        public DataComponent()
        {
            HashCode = HashCodeTypeTool.GetCachedHashCode<T>();
        }

        public event Action<IComponent, IComponent> OnPreUpdated;
        public event Action<IComponent> OnUpdated;

        public virtual ushort Id { get; }

        public int HashCode { get; }

        public void Dispose()
        {
            ReturnToPool();
        }

        public virtual void OnCreate()
        {
        }

        public virtual void ReturnToPool()
        {
        }

        protected DataComponent<T> GetInstanceFromPool()
        {
            return ComponentsTool.GetInstanceFromPool<DataComponent<T>>();
        }

        public virtual DataComponent<T> CopyTo(DataComponent<T> component)
        {
            return component;
        }

        public void PreUpdated(IComponent component)
        {
            // OnPreUpdated?.Invoke(this, component);
            EventsManager.Send(new ComponentPreUpdatedEvent
            {
                PreviousValue = this,
                FutureValue = component
            });
        }

        public void Updated()
        {
            // OnUpdated?.Invoke(this);

            EventsManager.Send(new ComponentUpdatedEvent { Value = this });
        }
    }
}