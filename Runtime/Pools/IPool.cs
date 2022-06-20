using System.Collections;

namespace DesertImage.Pools
{
    public interface IPool
    {
    }

    public interface IPool<T> : IPool where T : IPoolable
    {
        void Register(int count);
        IEnumerator AsyncRegister(int count);
        
        T GetInstance();
        void ReturnInstance(T instance);
    }

    public interface IMonoBehaviourPool<T> : IPool
    {
        void Register(T instance, int count);
        T GetInstance(T instance);
        void ReturnInstance(T instance);
    }
}