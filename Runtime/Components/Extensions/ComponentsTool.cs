using DesertImage.Pools;
using UnityEngine.SceneManagement;

namespace DesertImage.ECS
{
    public static class ComponentsTool
    {
        private static PoolComponents Pool => _pool ??= new PoolComponents();
        private static PoolComponents _pool;

        static ComponentsTool()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //reset instance when going to another scene
            _pool?.Clear();
            _pool = null;
        }

        /// <summary>
        /// Get new component instance from pool
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public static TComponent GetInstanceFromPool<TComponent>() where TComponent : IComponent, new()
        {
            return Pool.GetInstance<TComponent>();
        }

        public static TComponent AddComponentFromPool<TComponent>(this IComponentHolder componentHolder)
            where TComponent : class, IComponent, new()
        {
            var instance = Pool.GetInstance<TComponent>() /*new TComponent()*/;

            if (instance == null) return new TComponent();

            componentHolder?.Add(instance);

            return instance;
        }

        public static void ReturnToPool<TComponent>(this TComponent component)
            where TComponent : class, IComponent
        {
            Pool.ReturnInstance(component);
        }
    }
}