using UnityEngine;

namespace DesertImage.Pools
{
    public abstract class SimpleMonoPool<T> : Pool<T> where T : MonoBehaviour, IPoolable, new()
    {
        private readonly Transform _parent;

        public SimpleMonoPool(Transform parent)
        {
            _parent = parent;
        }

        public override T GetInstance()
        {
            var instance = base.GetInstance();

            instance.transform.SetParent(null);
            
            instance.gameObject.SetActive(true);

            return instance;
        }

        public override void ReturnInstance(T instance)
        {
            instance.transform.SetParent(_parent);

            instance.gameObject.SetActive(false);
            
            base.ReturnInstance(instance);
        }
    }
}