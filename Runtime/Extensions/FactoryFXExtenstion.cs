using DesertImage.ECS;
using DesertImage.Enums;
using Framework.FX;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DesertImage.Extensions
{
    public static class FactoryFXExtenstion
    {
        private static FactoryFx Factory => _factoryFx ??= Core.Instance.Get<FactoryFx>();

        private static FactoryFx _factoryFx;

        static FactoryFXExtenstion()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _factoryFx = null;
        }

        #region SPAWN

        public static EffectBase Spawn(this object sender, EffectsId id, Vector3 position)
        {
            return Factory.Spawn(id, position, Quaternion.identity, null);
        }

        public static EffectBase Spawn(this object sender, EffectsId id, Transform parent)
        {
            return Factory.Spawn(id, parent.position, parent.rotation, parent);
        }

        public static EffectBase Spawn(this object sender, EffectsId id, Vector3 position, Quaternion rotation)
        {
            return Factory.Spawn(id, position, rotation, null);
        }

        #endregion
    }
}