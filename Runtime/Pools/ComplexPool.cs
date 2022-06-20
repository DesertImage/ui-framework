using System.Collections.Generic;
using DesertImage.Extensions;

namespace DesertImage.Pools
{
    public interface IComplexPool
    {
        T GetInstance<T>() where T :  new();

        void Register<T>(T instance, int count) where T : class, new();

        void ReturnInstance<T>(T instance) where T : class;

        void Clear();
    }

    public abstract class ComplexPool : IComplexPool
    {
        protected readonly Dictionary<int, Stack<object>> Instances = new Dictionary<int, Stack<object>>();

        public virtual T GetInstance<T>() where T : new()
        {
            var hash = HashCodeTypeTool.GetCachedHashCode<T>();
            
            Instances.TryGetValue(hash, out var stack);

            var instance = (T) (stack != null && stack.Count > 0 && stack.Peek() != null
                ? stack.Pop()
                : CreateInstance<T>());

            GetStuff(instance);

            return instance;
        }

        protected virtual void GetStuff<T>(T objInstance)
        {
        }

        public virtual void Register<T>(T instance, int count) where T : class, new()
        {
            var hash = GetId(instance);

            if (!Instances.TryGetValue(hash, out var objStack))
            {
                objStack = new Stack<object>();

                Instances.Add(hash, objStack);
            }

            for (var i = 0; i < count; i++)
            {
                objStack.Push(CreateInstance<T>());

                Instances[hash] = objStack;
            }
        }

        public virtual void ReturnInstance<T>(T instance)  where T : class
        {
            var hash = HashCodeTypeTool.GetCachedHashCode<T>();

            if (Instances.TryGetValue(hash, out var stack))
            {
                stack.Push(instance);
            }
            else
            {
                var newStack = new Stack<object>();

                newStack.Push(instance);

                Instances.Add(hash, newStack);
            }

            ReturnStuff(instance);
        }

        public void Clear()
        {
            Instances.Clear();
        }

        protected virtual void ReturnStuff<T>(T instance)
        {
        }

        protected virtual T CreateInstance<T>() where T : new()
        {
            return new T();
        }

        protected virtual int GetId<T>(T obj)
        {
            return HashCodeTypeTool.GetCachedHashCode<T>();
        }
    }
}