using UnityEngine;

namespace DesertImage.ECS
{
    public interface IComponentWrapper
    {
        void Link(IComponentHolder componentHolder);
    }
    
    public abstract class ComponentWrapper : MonoBehaviour, IComponentWrapper
    {
        public abstract void Link(IComponentHolder componentHolder);
    }

    public class ComponentWrapper<T> : ComponentWrapper where T : IDataComponent
    {
        [SerializeField] protected T data;

        protected virtual T GetData()
        {
            return data;
        }

        public override void Link(IComponentHolder componentHolder)
        {
            componentHolder.Add(GetData()); 
        }
    }
}