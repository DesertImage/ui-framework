using DesertImage.ECS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DesertImage.Extensions
{
    public static class FactorySpawnExtenstion
    {
        private static FactorySpawn Factory => _factorySpawn ??= Core.Instance.Get<FactorySpawn>();

        private static FactorySpawn _factorySpawn;

        static FactorySpawnExtenstion()
        {
            SceneManager.sceneUnloaded += SceneManagerOnsceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            _factorySpawn = Core.Instance?.Get<FactorySpawn>();
        }

        private static void SceneManagerOnsceneUnloaded(Scene arg0)
        {
            _factorySpawn = null;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //reset instance when going to another scene
            _factorySpawn = null;
        }

        #region SPAWN

        public static GameObject Spawn(this object sender, ushort id, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            return Factory.Spawn(id, position, rotation, parent);
        }

        public static GameObject Spawn(this object sender, ushort id, Vector3 position, Transform parent = null)
        {
            return Factory.Spawn(id, position, Quaternion.identity, parent);
        }

        public static GameObject Spawn(this object sender, ushort id)
        {
            return Factory.Spawn(id, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(this object sender, ushort id)
        {
            return Factory.Spawn<T>(id, Vector3.zero);
        }

        public static T Spawn<T>(this object sender, ushort id, Vector3 position)
        {
            return Factory.Spawn<T>(id, position);
        }

        public static T Spawn<T>(this object sender, ushort id, Vector3 position, Quaternion rotation)
        {
            return Factory.Spawn<T>(id, position, rotation);
        }

        public static T Spawn<T>(this object sender, ushort id, Vector3 position, Quaternion rotation,
            Transform parent) where T : class
        {
            return Factory.Spawn<T>(id, position, rotation, parent);
        }

        public static T Spawn<T>(this object sender, ushort id, Vector3 position, Transform parent)
        {
            return Factory.Spawn<T>(id, position, Quaternion.identity, parent);
        }

        public static T Spawn<T>(this object sender, ushort id, Transform parent)
        {
            return Factory == null ? default : Factory.Spawn<T>(id, parent);
        }

        public static GameObject Spawn(this object sender, ushort id, Transform parent)
        {
            return Factory.Spawn(id, parent, Vector3.one);
        }

        public static GameObject Spawn(this object sender, ushort id, Transform parent, Vector3 scale)
        {
            return Factory.Spawn(id, parent, scale);
        }

        #endregion

        public static T GetPrefab<T>(this object sender, ushort id)
        {
            var prefab = Factory.GetPrefab(id);

            return prefab == null ? default : prefab.GetComponent<T>();
        }

        public static void ReturnToPool(this object sender, ref GameObject gameObject)
        {
            if (!gameObject) return;

            Factory?.ReturnInstance(gameObject);

            gameObject = null;
        }

        public static void ReturnToPool(this object sender, ref Transform transform)
        {
            if (!transform) return;

            Factory?.ReturnInstance(transform.gameObject);

            transform = null;
        }

        public static void ReturnToPool<T>(this object sender, ref T unityComponent) where T : Component
        {
            if (!unityComponent) return;

            Factory?.ReturnInstance(unityComponent.gameObject);

            unityComponent = null;
        }

        public static void ReturnToPool(this Transform transform)
        {
            if (!transform) return;

            Factory?.ReturnInstance(transform.gameObject);
        }
    }
}