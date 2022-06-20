using DesertImage;
using DesertImage.ECS;
using UnityEngine;

namespace Framework.FX
{
    public class EffectBase : MonoBehaviour, IFX, IPoolable
    {
        public virtual void Play()
        {
            
        }

        public virtual void Stop()
        {
            
        }

        public virtual void OnCreate()
        {
            
        }

        public virtual void ReturnToPool()
        {
            Stop();

            Core.Instance.Get<FactoryFx>().ReturnInstance(this);
        }
    }
}